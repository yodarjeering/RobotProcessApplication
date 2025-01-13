using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotProcessApplication.Services;
using System.Collections.Generic;
using RobotProcessApplication.Models;
using System.Threading;

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
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken cancellationToken = cts.Token;

            try
            {
                List<RobotProcessApplication.Services.MouseAction> mouseActions;

                // ロックをかけてコレクションを取得
                lock (_lockObject)
                {
                    mouseActions = recorder.GetMouseActions();
                }

                if (mouseActions.Count == 0)
                {
                    MessageBox.Show("再生するアクションがありません。");
                    return;
                }

                DateTime previousTime = DateTime.MinValue;

                // mouseActionsのコピーを作成
                var actionsToPlay = new List<RobotProcessApplication.Services.MouseAction>(mouseActions);

                // MousePlayerのインスタンスを作成（キーボードアクションは渡さない）
                var mousePlayer = new MousePlayer(mouseActions);

                // actionsToPlay全体を一度に再生
                await mousePlayer.PlayActionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}");
            }
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