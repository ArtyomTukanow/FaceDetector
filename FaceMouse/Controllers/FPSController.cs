using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceMouse.Controllers
{
    delegate void FpsDelegate(int fps);
    class FPSController
    {
        public static event FpsDelegate FpsRecalc;

        private static Thread _fpsTrackingThread;

        public static void Start() {
            _fpsTrackingThread = new Thread(FpsTrak);
            _fpsTrackingThread.Start();
        }

        private static int _frames = 0;
        public static void AddFrame()
        {
            _frames++;
        }

        private static void FpsTrak()
        {
            while (true)
            {
                FpsRecalc(_frames);
                _frames = 0;
                Thread.Sleep(1000);
            }
        }



        public static void Exit() {
            _fpsTrackingThread.Abort();
        }
    }
}
