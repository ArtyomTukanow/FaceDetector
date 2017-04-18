using FaceMouse.MouseCaptureModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceMouse.Controllers
{
    class MouseController
    {
        private static Thread _mouseCaptureThread;

        public static void Start() {
            _mouseCaptureThread = new Thread(MouseCaptureTread);
        }

        

        public static void CaptureMouse(object k = null, object e = null)
        {
            ModuleController.Form.Activate();
            if (!_mouseCaptureThread.IsAlive)
                _mouseCaptureThread.Start();
            isMouseMoving = !isMouseMoving;
        }
        public static void Exit()
        {
            _mouseCaptureThread.Abort();
        }



        private static ClickStatus _click;
        public static bool isMouseMoving;
        private static HotKey detectHotKey;
        private static HotKey captureHotKey;
        private static void MouseCaptureTread()
        {
            while (true)
            {
                Thread.Sleep(40);

                if (isMouseMoving)
                {
                    switch (_click)
                    {
                        case ClickStatus.none:
                            Point currMousePoint = MouseIteraction.GetCursorPos();
                            Point mouseDirr = MouseCapture.GetCurrentDirrection();
                            if (Math.Abs(mouseDirr.X) > 1)
                                currMousePoint.X += mouseDirr.X;
                            if (Math.Abs(mouseDirr.Y) > 1)
                                currMousePoint.Y += mouseDirr.Y;
                            MouseIteraction.MoveMouse(currMousePoint);
                            break;
                        case ClickStatus.doubleLeft:
                            MouseIteraction.SendMouseDoubleClick();
                            _click = ClickStatus.none;
                            break;
                        case ClickStatus.left:
                            MouseIteraction.DoMouseClick();
                            _click = ClickStatus.none;
                            break;
                        case ClickStatus.right:
                            MouseIteraction.SendMouseRightclick();
                            _click = ClickStatus.none;
                            break;
                    }
                }
            }
        }
    }
}
