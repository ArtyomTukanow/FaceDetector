namespace FaceMouse
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cameraBox = new System.Windows.Forms.PictureBox();
            this.detectFaceButton = new System.Windows.Forms.Button();
            this.mouseCaptureButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cameraBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraBox
            // 
            this.cameraBox.Location = new System.Drawing.Point(12, 12);
            this.cameraBox.Name = "cameraBox";
            this.cameraBox.Size = new System.Drawing.Size(433, 377);
            this.cameraBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cameraBox.TabIndex = 1;
            this.cameraBox.TabStop = false;
            // 
            // detectFaceButton
            // 
            this.detectFaceButton.Location = new System.Drawing.Point(273, 396);
            this.detectFaceButton.Name = "detectFaceButton";
            this.detectFaceButton.Size = new System.Drawing.Size(75, 23);
            this.detectFaceButton.TabIndex = 2;
            this.detectFaceButton.Text = "Распознать";
            this.detectFaceButton.UseVisualStyleBackColor = true;
            this.detectFaceButton.Click += new System.EventHandler(this.DetectButton_Click);
            // 
            // mouseCaptureButton
            // 
            this.mouseCaptureButton.Location = new System.Drawing.Point(354, 396);
            this.mouseCaptureButton.Name = "mouseCaptureButton";
            this.mouseCaptureButton.Size = new System.Drawing.Size(91, 23);
            this.mouseCaptureButton.TabIndex = 3;
            this.mouseCaptureButton.Text = "Захват мыши";
            this.mouseCaptureButton.UseVisualStyleBackColor = true;
            this.mouseCaptureButton.Click += new System.EventHandler(this.mouseCaptureButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 431);
            this.Controls.Add(this.mouseCaptureButton);
            this.Controls.Add(this.detectFaceButton);
            this.Controls.Add(this.cameraBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cameraBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox cameraBox;
        private System.Windows.Forms.Button detectFaceButton;
        private System.Windows.Forms.Button mouseCaptureButton;
    }
}

