using System;
using System.Drawing;
using System.Windows.Forms;
using FaceMouse.MouseCaptureModule;
using FaceMouse.Controllers;

namespace FaceMouse
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FPSController.FpsRecalc += new FpsDelegate(delegate(int fps) {
                fpsText.Invoke((MethodInvoker)delegate
                {
                    ModuleController.Form.fpsText.Text = "FPS: " + fps;
                });
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ModuleController.Exit();
            FPSController.Exit();
        }

        private void DetectButton_Click(object sender, EventArgs e)
        {
            ModuleController.Detect();
        }

        private void mouseCaptureButton_Click(object sender, EventArgs e)
        {
            ModuleController.CaptureMouse();
        }
    }
}