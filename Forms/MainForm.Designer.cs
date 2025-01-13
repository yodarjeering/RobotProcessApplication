using System.Windows.Forms;

namespace RobotProcessApplication.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnStartRecording;
        private Button btnStopRecording;
        private Button btnPlayRecording;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnStartRecording = new Button();
            this.btnStopRecording = new Button();
            this.btnPlayRecording = new Button();
            this.SuspendLayout();
            // this.KeyPreview = true;
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Location = new System.Drawing.Point(50, 50);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(150, 30);
            this.btnStartRecording.Text = "録画開始";
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Location = new System.Drawing.Point(50, 100);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(150, 30);
            this.btnStopRecording.Text = "録画停止";
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // btnPlayRecording
            // 
            this.btnPlayRecording.Location = new System.Drawing.Point(50, 150);
            this.btnPlayRecording.Name = "btnPlayRecording";
            this.btnPlayRecording.Size = new System.Drawing.Size(150, 30);
            this.btnPlayRecording.Text = "再生";
            this.btnPlayRecording.Click += new System.EventHandler(this.btnPlayRecording_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.btnPlayRecording);
            this.Name = "MainForm";
            this.Text = "メインフォーム";
            this.ResumeLayout(false);
        }
    }
}