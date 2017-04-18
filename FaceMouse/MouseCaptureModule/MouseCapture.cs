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

        public static Point GetCurrentDirrection()
        {
            Point dirr = new Point();
            dirr.Y = Convert.ToInt32(NoseDif*100);
            dirr.X = Convert.ToInt32(EyeLeftDif*100);
            return dirr;
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