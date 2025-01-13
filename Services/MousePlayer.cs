using WindowsInput;
using WindowsInput.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using RobotProcessApplication.Models;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RobotProcessApplication.Services
{
    public class MousePlayer
    {
        private InputSimulator _simulator;
        private List<MouseAction> _mouseActions;

        public MousePlayer(List<MouseAction> mouseActions)
        {
            _simulator = new InputSimulator();
            _mouseActions = new List<MouseAction>(mouseActions);
        }

        public async Task PlayActionsAsync(CancellationToken cancellationToken)
        {
            DateTime previousTime = DateTime.MinValue;
            var mainWindow = Form.ActiveForm; // アクティブなウィンドウを取得

            // マウス操作の再生
            foreach (var action in _mouseActions)
            {
                // キャンセルが要求された場合、メソッドを終了
                cancellationToken.ThrowIfCancellationRequested();
                Debug.Print("MousePlayer: " + action.Position.X + "," + action.Position.Y);

                if (previousTime != DateTime.MinValue)
                {
                    var delay = action.Timestamp - previousTime;
                    if (delay.TotalMilliseconds > 0)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }

                previousTime = action.Timestamp;

                if (action.Type == MouseAction.ActionType.Move)
                {
                    // ウィンドウの位置を考慮して座標を調整
                    if (mainWindow != null)
                    {
                        var adjustedPosition = new Point(action.Position.X + mainWindow.Location.X, action.Position.Y + mainWindow.Location.Y);
                        Cursor.Position = adjustedPosition;
                    }
                    else
                    {
                        Cursor.Position = action.Position; // ウィンドウが取得できない場合はそのまま
                    }
                }
                else if (action.Type == MouseAction.ActionType.Click)
                {
                    _simulator.Mouse.LeftButtonClick();
                }
            }
        }
    }
}