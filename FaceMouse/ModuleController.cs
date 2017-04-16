using System.Drawing;
using System.Threading;
using FaceMouse.ComputerVisionModule;
using FaceMouse.MouseCaptureModule;

namespace FaceMouse
{
    public class ModuleController
    {
        public static Form1 Form;
        private static Thread _faceTrackingThread;
        private static Thread _mouseCaptureThread;

        public static void Start()
        {
            Form = new Form1();
            _faceTrackingThread = new Thread(ComputerVisionThread);
            _mouseCaptureThread = new Thread(MouseCaptureTread);
        }

        public static void Detect()
        {
            if (!_faceTrackingThread.IsAlive)
                _faceTrackingThread.Start();
            reDetect = true;
        }

        public static void ReMoveMouse()
        {
            if (!_mouseCaptureThread.IsAlive)
                _mouseCaptureThread.Start();
            isMouseMoving = !isMouseMoving;
        }

        public static void Exit()
        {
            _faceTrackingThread.Abort();
            _mouseCaptureThread.Abort();
        }

        private static bool reDetect;
        private static void ComputerVisionThread()
        {
            while (true)
            {
                Point eyeLeft = new Point();
                Point eyeRight = new Point();
                Point nose = new Point();
                if (reDetect)
                {
                    reDetect = false;
                    Vision.DetectFace(ref eyeLeft, ref eyeRight, ref nose);
                    MouseCapture.SetTriangle(eyeLeft, eyeRight, nose);
                }
                else
                {
                    Form.cameraBox.Image = Vision.TrackeFace(ref eyeLeft, ref eyeRight, ref nose);
                    MouseCapture.UpdateTriangle(eyeLeft, eyeRight, nose);
                }
            }
        }

        public static bool isMouseMoving;
        private static void MouseCaptureTread()
        {
            while (true)
            {
                Thread.Sleep(50);

                if (isMouseMoving)
                {
                    Point currMousePoint = MouseIteraction.GetCursorPos();
                    Point mouseDirr = MouseCapture.GetCurrentDirrection();
                    currMousePoint.X += mouseDirr.X;
                    currMousePoint.Y += mouseDirr.Y;
                    MouseIteraction.MoveMouse(currMousePoint);
                }
            }
        }
    }
}