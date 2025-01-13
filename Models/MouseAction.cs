using System;
using System.Drawing;
using System.Diagnostics;

namespace RobotProcessApplication.Models
{
    public class MouseAction
    {
        public enum ActionType
        {
            Move,
            Click,
            DoubleClick,
            RightClick
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