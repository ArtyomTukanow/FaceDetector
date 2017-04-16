using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using FaceMouse.ComputerVisionModule;
using FaceMouse.MouseCaptureModule;

namespace FaceMouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ModuleController.Exit();
        }

        private void DetectButton_Click(object sender, EventArgs e)
        {
            ModuleController.Detect();
        }

        private void mouseCaptureButton_Click(object sender, EventArgs e)
        {
            Point point = MouseIteraction.GetCursorPos();
            MouseIteraction.SetCursorPos(point.X + 50, point.Y + 50);
        }
    }
}