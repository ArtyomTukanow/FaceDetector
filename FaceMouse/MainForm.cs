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