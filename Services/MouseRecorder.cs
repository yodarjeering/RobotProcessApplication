using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RobotProcessApplication.Models;
using System.Diagnostics;

namespace RobotProcessApplication.Services
{
    public class MouseRecorder
    {
        private List<MouseAction> _mouseActions;
        private bool _isRecording;

        public MouseRecorder()
        {
            _mouseActions = new List<MouseAction>();
            _isRecording = false;
        }

        public void StartRecording()
        {
            _mouseActions.Clear();
            _isRecording = true;
        }

        public void StopRecording()
        {
            _isRecording = false;
        }

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