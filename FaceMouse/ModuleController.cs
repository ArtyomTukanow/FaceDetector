using System.Threading;
using FaceMouse.ComputerVisionModule;

namespace FaceMouse
{
    public class ModuleController
    {
        public static Form1 Form;
        private static Thread _faceDetectionThread;

        public static void Start()
        {
            Form = new Form1();
            _faceDetectionThread = new Thread(ComputerVision);
        }

        public static void Detect()
        {
            if (!_faceDetectionThread.IsAlive)
                _faceDetectionThread.Start();
            reDetect = true;
        }

        public static void Exit()
        {
            _faceDetectionThread.Abort();
        }

        private static bool reDetect;
        private static void ComputerVision()
        {
            while (true)
            {
                if (reDetect)
                {
                    reDetect = false;
                    Vision.DetectFace();
                }
                Form.cameraBox.Image = Vision.TrackeFace();
            }
        }
    }
}