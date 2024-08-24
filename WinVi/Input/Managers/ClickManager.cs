using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using WinVi.Input.Utilities;

namespace WinVi.Input
{
    internal class ClickManager
    {

        private static readonly Lazy<ClickManager> _instance = new Lazy<ClickManager>(() => new ClickManager(), true);

        private ClickManager()
        {
        }

        internal static ClickManager Instance = _instance.Value;

        public void Click(Rect rect, bool invokeRightClick)
        {
            // int absoluteX = Convert.ToInt32(((rect.X + (rect.Width / 2)) - _screen.Bounds.Left) * 65536 / _screen.Bounds.Width);
            // int absoluteY = Convert.ToInt32(((rect.Y + (rect.Height / 2)) - _screen.Bounds.Top) * 65536 / _screen.Bounds.Height);

            // Store initial cursor position

            var downFlag = MouseClickUtilities.MouseEventFlags.Absolute | 
                (invokeRightClick ? MouseClickUtilities.MouseEventFlags.RightDown : MouseClickUtilities.MouseEventFlags.LeftDown);
            var upFlag = MouseClickUtilities.MouseEventFlags.Absolute | 
                (invokeRightClick ? MouseClickUtilities.MouseEventFlags.RightUp : MouseClickUtilities.MouseEventFlags.LeftUp);

            var inputs = new[]
            {
                CreateMouseInput(0, 0, downFlag),
                CreateMouseInput(0, 0, upFlag)
            };

            // Move cursor to a needed rect position

            // invoke mouse click
            MouseClickUtilities.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(MouseClickUtilities.Input)));

            // Return Cursor to its initial position
        }

        private MouseClickUtilities.Input CreateMouseInput(int x, int y, MouseClickUtilities.MouseEventFlags flags, uint mouseScrollAmount = 0)
        {
            return new MouseClickUtilities.Input
            {
                type = MouseClickUtilities.SendInputEventType.Mouse,
                mouseInput = new MouseClickUtilities.MouseInput
                {
                    dx = x,
                    dy = y,
                    mouseData = mouseScrollAmount,
                    dwFlags = flags,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero,
                },
            };
        }

        /*
        DEBUG:
        Add-Type -AssemblyName System.Windows.Forms
        while (1) {
            $X = [System.Windows.Forms.Cursor]::Position.X
            $Y = [System.Windows.Forms.Cursor]::Position.Y

            Write-Host -NoNewline "`rX: $X | Y: $Y"
        }

        powershell script to live check pixel screen coordinates
        */
    }
}
