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
        public TimeSpan Timestamp { get; set; }

        public MouseAction(ActionType actionType, Point position, TimeSpan timestamp)
        {
            Type = actionType;
            Position = position;
            Timestamp = timestamp;
        }
    }
}