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

        // Windows APIの定義
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc _mouseProc;
        private IntPtr _hookID = IntPtr.Zero;

        public MouseRecorder()
        {
            _mouseActions = new List<MouseAction>();
            _isRecording = false;
            _mouseProc = HookCallback;
        }

        public void StartRecording()
        {
            _mouseActions.Clear();
            _isRecording = true;
            _hookID = SetHook(_mouseProc);
        }

        public void StopRecording()
        {
            _isRecording = false;
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
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
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
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
    }

    public class MouseAction
    {
        public enum ActionType
        {
            Move,
            Click
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