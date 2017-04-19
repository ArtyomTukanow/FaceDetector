using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FaceMouse.ComputerVisionModule
{
    public partial class Vision
    {
        private static Image<Gray, Byte> _lastLeftEye;
        private static Image<Gray, Byte> _lastRightEye;

        private static double TrakeLeftEye(Mat imageMat, Rectangle leftEyeRect)
        {
            Mat part = new Mat(imageMat, leftEyeRect);
            Image<Gray, Byte> currEye = part.ToImage<Gray, Byte>();
            double diff = 0;
            if (_lastLeftEye != null)
            {
                Gray lastSum = _lastLeftEye.GetSum();
                Gray currSum = currEye.GetSum();
                diff = currSum.Intensity - lastSum.Intensity;
            }
            _lastLeftEye = currEye;
            return diff;
        }

        private static double TrakeRightEye(Mat imageMat, Rectangle leftEyeRect)
        {
            Mat part = new Mat(imageMat, leftEyeRect);
            Image<Gray, Byte> currEye = part.ToImage<Gray, Byte>();
            double diff = 0;
            if (_lastRightEye != null)
            {
                Gray lastSum = _lastRightEye.GetSum();
                Gray currSum = currEye.GetSum();
                diff = currSum.Intensity - lastSum.Intensity;
            }
            _lastRightEye = currEye;
            return diff;
        }
    }
}
