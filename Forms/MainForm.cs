using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotProcessApplication.Services;
using System.Collections.Generic;
using RobotProcessApplication.Models;
using System.Threading;
using System.Diagnostics;

namespace RobotProcessApplication.Forms
{
    public partial class MainForm : Form
    {
        private MouseRecorder recorder;
        private MousePlayer player;
        private bool isPlaying = false;
        // private List<MouseAction> _mouseActions = new List<MouseAction>();
        private readonly object _lockObject = new object();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            recorder = new MouseRecorder();
            recorder.StartRecording();
            MessageBox.Show("録画開始しました");
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            recorder.StopRecording();
            MessageBox.Show("録画終了しました");
        }

        private async void btnPlayRecording_Click(object sender, EventArgs e)
        {
            isPlaying = true;
            btnStopRecording.Enabled = false;

            // 再生処理
            player = new MousePlayer(recorder.Actions);

            DateTime previousTime = DateTime.MinValue;

            foreach (var action in recorder.Actions)
            {
                if (previousTime != DateTime.MinValue)
                {
                    var delay = action.Timestamp - previousTime;
                    if (delay.TotalMilliseconds > 0)
                    {
                        await Task.Delay(delay);
                    }
                }

                previousTime = action.Timestamp;

                Cursor.Position = action.Position;
                player.PlayAction(action);
            }

            btnStopRecording.Enabled = true;
            isPlaying = false;

            // 再生終了後にフックを解除
            recorder.StopRecording();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (recorder == null)
            {
                recorder = new MouseRecorder();
                recorder.StartRecording();
            }
            recorder.RecordMouseMove(e.Location);
            Debug.Print("Recorded Mouse Move: " + e.Location);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            recorder.RecordMouseClick(e.Location);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // recorder.RecordKeyDown(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            // recorder.RecordKeyUp(e.KeyCode);
        }
    }
}