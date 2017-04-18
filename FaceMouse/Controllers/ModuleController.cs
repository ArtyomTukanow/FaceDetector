﻿using System.Drawing;
using System.Threading;
using FaceMouse.ComputerVisionModule;
using FaceMouse.MouseCaptureModule;
using System;
using System.Windows.Forms;

namespace FaceMouse.Controllers
{
    public class ModuleController
    {
        public static MainForm Form;
        private static Thread _faceTrackingThread;
        private static Thread _mouseCaptureThread;

        public static void Start()
        {
            Form = new MainForm();
            _faceTrackingThread = new Thread(ComputerVisionThread);
            _mouseCaptureThread = new Thread(MouseCaptureTread);

            detectHotKey = new HotKey();
            detectHotKey.KeyModifier = HotKey.KeyModifiers.Control;
            detectHotKey.Key = System.Windows.Forms.Keys.D;
            detectHotKey.HotKeyPressed += Detect;

            captureHotKey = new HotKey();
            captureHotKey.KeyModifier = HotKey.KeyModifiers.Control;
            captureHotKey.Key = System.Windows.Forms.Keys.M;
            captureHotKey.HotKeyPressed += CaptureMouse;
        }

        public static void Detect(object k = null, object e = null)
        {
            Form.Activate();
            if (!_faceTrackingThread.IsAlive)
                _faceTrackingThread.Start();
            reDetect = true;
        }

        public static void CaptureMouse(object k = null, object e = null)
        {
            Form.Activate();
            if (!_mouseCaptureThread.IsAlive)
                _mouseCaptureThread.Start();
            isMouseMoving = !isMouseMoving;
        }

        private static bool reDetect;
        private static void ComputerVisionThread()
        {
            while (true)
            {
                FPSController.AddFrame();
                Point eyeLeft = new Point();
                Point eyeRight = new Point();
                Point nose = new Point();
                if (reDetect)
                {
                    reDetect = false;
                    Vision.DetectFace(ref eyeLeft, ref eyeRight, ref nose);
                    MouseCapture.SetTriangle(eyeLeft, eyeRight, nose);
                    MouseCapture.Position = MouseIteraction.GetCursorPos();
                }
                else
                {
                    Form.cameraBox.Image = Vision.TrackeFace(ref eyeLeft, ref eyeRight, ref nose, ref _click);
                    MouseCapture.UpdateTriangle(eyeLeft, eyeRight, nose);
                }
            }
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
                    //Point currMousePoint = MouseIteraction.GetCursorPos();
                    //Point mouseDirr = MouseCapture.GetCurrentDirrection();
                    //if(Math.Abs(mouseDirr.X) > 1)
                    //    currMousePoint.X += mouseDirr.X;
                    //if (Math.Abs(mouseDirr.Y) > 1)
                    //    currMousePoint.Y += mouseDirr.Y;
                    //MouseIteraction.MoveMouse(currMousePoint);
                    switch (_click)
                    {
                        case ClickStatus.none:
                            MouseCapture.UpdatePosition();
                            MouseIteraction.MoveMouse(MouseCapture.Position);
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



        public static void Exit()
        {
            _faceTrackingThread.Abort();
            _mouseCaptureThread.Abort();
        }
    }
}