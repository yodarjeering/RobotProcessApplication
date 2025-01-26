using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RobotProcessApplication.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RobotProcessApplication.Services
{
    public class MouseRecorder
    {
        private List<MouseAction> _mouseActions;
        private bool _isRecording;
        private Stopwatch _stopwatch;
        private IntPtr _mouseHookId = IntPtr.Zero;

        // Windows APIの定義
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc _proc;

        public MouseRecorder()
        {
            _mouseActions = new List<MouseAction>();
            _isRecording = false;
            _stopwatch = new Stopwatch();
        }

        public void StartRecording()
        {
            _mouseActions.Clear();
            _isRecording = true;
            _stopwatch.Start();
            SetHook();
        }

        public void StopRecording()
        {
            _isRecording = false;
            _stopwatch.Stop();
            
            if (_mouseHookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_mouseHookId);
                _mouseHookId = IntPtr.Zero; // フックIDをリセット
            }
            
            Debug.Print("録画停止");
        }

        private void SetHook()
        {
            _proc = HookCallback;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _mouseHookId = SetWindowsHookEx(WH_MOUSE_LL, _proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                
                if (_mouseHookId == IntPtr.Zero)
                {
                    // エラーハンドリング
                    int errorCode = Marshal.GetLastWin32Error();
                    Debug.Print($"フックの設定に失敗しました。エラーコード: {errorCode}");
                }
            }
            Debug.Print("グローバルマウスフック開始");
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && _isRecording)
            {
                var mouseAction = (MouseAction.ActionType)wParam;
                var mousePoint = Marshal.PtrToStructure<Point>(lParam);

                if (mouseAction == MouseAction.ActionType.Move)
                {
                    RecordMouseMove(mousePoint);
                }
                else if (mouseAction == MouseAction.ActionType.Click)
                {
                    RecordMouseClick(mousePoint);
                }
            }
            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
        }

        // Windows APIの関数
        private const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public void RecordMouseMove(Point position)
        {
            if (!_isRecording) return;

            var timestamp = DateTime.Now;
            _mouseActions.Add(new MouseAction(MouseAction.ActionType.Move, position, timestamp));
            Debug.Print("RecordMouseMove: " + position);
        }

        public void RecordMouseClick(Point position)
        {
            if (!_isRecording) return;

            var timestamp = DateTime.Now;
            _mouseActions.Add(new MouseAction(MouseAction.ActionType.Click, position, timestamp));
        }

        public List<MouseAction> GetMouseActions()
        {
            return _mouseActions;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isRecording)
            {
                var screenLocation = Cursor.Position;
                var timestamp = _stopwatch.Elapsed;
                _mouseActions.Add(new MouseAction(MouseAction.ActionType.Move, screenLocation, timestamp));
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (_isRecording)
            {
                MouseAction.ActionType actionType = MouseAction.ActionType.Click;

                if (e.Button == MouseButtons.Right)
                {
                    actionType = MouseAction.ActionType.RightClick;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (e.Clicks == 2)
                    {
                        actionType = MouseAction.ActionType.DoubleClick;
                    }
                }

                var screenLocation = Cursor.Position;
                var timestamp = _stopwatch.Elapsed;
                _mouseActions.Add(new MouseAction(actionType, screenLocation, timestamp));
            }
        }

        public IReadOnlyList<MouseAction> Actions => _mouseActions.AsReadOnly();
    }

    public class MouseAction
    {
        public enum ActionType
        {
            Move,
            Click,
            RightClick,
            DoubleClick
        }

        public ActionType Type { get; set; }
        public Point Position { get; set; }
        public DateTime Timestamp { get; set; }

        public MouseAction(ActionType type, Point position, DateTime timestamp)
        {
            Type = type;
            Position = position;
            Timestamp = timestamp;
        }
    }
}