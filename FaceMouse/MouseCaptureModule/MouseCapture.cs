using System;
using System.Drawing;

namespace FaceMouse.MouseCaptureModule
{
    public class MouseCapture
    {
        //Standart Angles
        private static double sEyeLeft;
        private static double sNose;
        //Current Angles
        private static double cEyeLeft;
        private static double cNose;

        private static int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        private static int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private static double _exactPosX;
        private static double _exactPosY;
        public static double exactPosX {
            set 
            {
                if (value < 0) value = 0;
                else if (value > screenWidth) value = screenWidth;
                _exactPosX = value;
            }
            get
            {
                return _exactPosX;
            }
        }
        public static double exactPosY
        {
            set
            {
                if (value < 0) value = 0;
                else if (value > screenHeight) value = screenHeight;
                _exactPosY = value;
            }
            get
            {
                return _exactPosY;
            }
        }

        public static Point Position
        {
            set
            {
                exactPosX = value.X;
                exactPosY = value.Y;
            }
            get
            {
                return new Point(Convert.ToInt32(exactPosX), Convert.ToInt32(exactPosY));
            }
        }

        public static void UpdatePosition()
        {
            exactPosY += NoseDif * Settings.sensitivity;
            exactPosX += EyeLeftDif * Settings.sensitivity;
        }


        private static double EyeLeftDif { get { return sEyeLeft - cEyeLeft; }}
        private static double NoseDif { get { return sNose - cNose; }}

        public static void SetTriangle(Point eyeLeft, Point eyeRight, Point nose)
        {
            sEyeLeft = AngleCalculate(nose, eyeLeft, eyeRight);
            sNose = AngleCalculate(eyeRight, nose, eyeLeft);

            cEyeLeft = sEyeLeft;
            cNose = sNose;
        }

        public static void UpdateTriangle(Point eyeLeft, Point eyeRight, Point nose)
        {
            cEyeLeft = AngleCalculate(nose, eyeLeft, eyeRight);
            cNose = AngleCalculate(eyeRight, nose, eyeLeft);
        }

        private static double AngleCalculate(Point pL, Point pM, Point pR)
        {
            Point v1 = new Point(pL.X - pM.X, pL.Y - pM.Y);
            Point v2 = new Point(pR.X - pM.X, pR.Y - pM.Y);

            double d1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
            double d2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);
            return Math.Acos((v1.X * v2.X + v1.Y * v2.Y) / (d1 * d2));
        }
    }
}