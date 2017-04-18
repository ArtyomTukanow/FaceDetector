using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Tracking;
using Emgu.CV.Util;
using FaceMouse.MouseCaptureModule;
using FaceMouse.Controllers;

namespace FaceMouse.ComputerVisionModule
{
    public partial class Vision
    {

        public static VideoCapture Capture = new VideoCapture();

        private static MultiTracker _tracker;

        private static VectorOfRect _vectorOfRect;
        public static VectorOfRect VectorOfRect
        {
            set
            {
                _tracker = new MultiTracker("KCF");
                foreach (Rectangle rect in value.ToArray())
                    _tracker.Add(new Image<Bgr, Byte>(Capture.QueryFrame().Bitmap).Mat, rect);
                _vectorOfRect = value;
            }
            get { return _vectorOfRect; }
        }

        /// <summary>
        /// Распознает лицо с двумя глазами и одним носом
        /// </summary>
        /// <param name="eyeLeft">Центр левого глаза</param>
        /// <param name="eyeRight">Центр правого глаза</param>
        /// <param name="nose">Центр носа</param>
        public static void DetectFace(ref Point eyeLeft, ref Point eyeRight, ref Point nose)
        {
            Rectangle[] rects = new Rectangle[3];

            while (true)
            {
                IImage currFrame = Capture.QueryFrame().ToImage<Bgr, Byte>();

                long detectionTime;
                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();
                List<Rectangle> noses = new List<Rectangle>();

                FaceDetector(
                    currFrame, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml", "Nariz.xml",
                    faces, eyes, noses,
                    out detectionTime);

                ModuleController.Form.cameraBox.Image = Capture.QueryFrame().Bitmap;

                if (noses.Count < 1 || eyes.Count != 2)
                    continue;

                foreach (Rectangle face in faces)
                    CvInvoke.Rectangle(currFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                Rectangle currNose = noses[0];
                foreach (Rectangle noseRectangle in noses)
                    if (currNose.Location.Y > noseRectangle.Location.Y)
                        currNose = noseRectangle;

                rects[0] = NewRectangle(currNose, 64, 64);

                for (int i = 0; i < 2; i++)
                    rects[i + 1] = NewRectangle(eyes[i], 64, 64);
                break;
            }
            nose = Center(rects[0]);
            if (rects[1].X < rects[2].X)
            {
                var h = rects[1];
                rects[1] = rects[2];
                rects[2] = h;
            }
            eyeLeft = Center(rects[1]);
            eyeRight = Center(rects[2]);
            VectorOfRect = new VectorOfRect(rects);
        }

        /// <summary>
        /// Возвращает изображение с точками отслеживания
        /// </summary>
        /// <param name="eyeLeft">Центр левого глаза</param>
        /// <param name="eyeRight">Центр правого глаза</param>
        /// <param name="nose">Центр носа</param>
        /// <returns>Возвращает изображение</returns>
        public static Bitmap TrackeFace(ref Point eyeLeft, ref Point eyeRight, ref Point nose, ref ClickStatus click)
        {
            lock (_vectorOfRect)
            {
                IImage currFrame = Capture.QueryFrame().ToImage<Bgr, Byte>();
                Image<Bgr, Byte> currFrameImg = Capture.QueryFrame().ToImage<Bgr, Byte>();
                _tracker.Update(Capture.QueryFrame(), _vectorOfRect);
                Rectangle[] rects = _vectorOfRect.ToArray();
                nose = Center(rects[0]);
                eyeLeft = Center(rects[1]);
                eyeRight = Center(rects[2]);

                List<Rectangle> detectedLeftEyes = new List<Rectangle>();
                long detectionTime;
                EyeDetector(currFrame, "haarcascade_one_eye.xml", NewRectangle(eyeLeft, 64, 64), detectedLeftEyes, out detectionTime);

                List<Rectangle> detectedRightEyes = new List<Rectangle>();
                long detectionTime2;
                EyeDetector(currFrame, "haarcascade_one_eye.xml", NewRectangle(eyeRight, 64, 64), detectedRightEyes, out detectionTime2);

                foreach (Rectangle eye in detectedLeftEyes)
                    CvInvoke.Rectangle(currFrame, eye, new Bgr(Color.Red).MCvScalar, 2);
                foreach (Rectangle eye in detectedRightEyes)
                    CvInvoke.Rectangle(currFrame, eye, new Bgr(Color.Blue).MCvScalar, 2);

                foreach (Rectangle rect in rects)
                    CvInvoke.Rectangle(currFrame, NewRectangle(rect, 4, 4), new Bgr(Color.Green).MCvScalar, 2);

                if (detectedLeftEyes.Count == 0 && detectedRightEyes.Count == 0)
                {
                    click = ClickStatus.doubleLeft;
                }
                else if (detectedLeftEyes.Count == 0)
                {
                    click = ClickStatus.left;
                }
                else if (detectedRightEyes.Count == 0)
                {
                    click = ClickStatus.right;
                }
                else
                {
                    click = ClickStatus.none;
                }

                return currFrame.Bitmap;
            }
        }





        private static Point Center(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width/2, rectangle.Y + rectangle.Height/2);
        }

        private static Point Left(Point center, int width, int height)
        {
            return new Point(center.X - width/2, center.Y - height/2);
        }

        private static Rectangle NewRectangle(Rectangle rectangle, int width, int height)
        {
            return new Rectangle(Left(Center(rectangle), width, height), new Size(width, height));
        }

        private static Rectangle NewRectangle(Point center, int width, int height)
        {
            return new Rectangle(Left(center, width, height), new Size(width, height));
        }
    }
}