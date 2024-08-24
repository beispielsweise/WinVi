using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using WinVi.Input.Utilities;

namespace WinVi.Input
{
    internal class ClickManager
    {
        private static readonly Lazy<ClickManager> _instance = new Lazy<ClickManager> (() => new ClickManager(), true);
        private static Screen _screen = null;

        private ClickManager ()
        {
            _screen = Screen.PrimaryScreen;
        }

        internal static ClickManager Instance = _instance.Value;

        /// <summary>
        /// Emulates a mouse button click at a specific position. isRightClick determines if a left click action should be pressed, or the right one
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rightClick"></param>
        public void Click(Rect rect, bool rightClick)
        {
            // Calculate the absolute coordinates
            int absoluteX = Convert.ToInt32((rect.X - _screen.Bounds.Left) * 65536 / _screen.Bounds.Width);
            int absoluteY = Convert.ToInt32((rect.Y - _screen.Bounds.Top) * 65536 / _screen.Bounds.Height);

            // Create a list to hold the input events
            var inputs = new MouseClickUtilities.Input[2];

            // Right mouse button down
            inputs[0] = new MouseClickUtilities.Input
            {
                type = MouseClickUtilities.SendInputEventType.Mouse,
                mouseInput = new MouseClickUtilities.MouseInput
                {
                    dx = absoluteX,
                    dy = absoluteY,
                    mouseData = 0,
                    dwFlags = MouseClickUtilities.MouseEventFlags.Absolute | 
                              (rightClick ? MouseClickUtilities.MouseEventFlags.RightDown : MouseClickUtilities.MouseEventFlags.LeftDown),
                    time = 0,
                    dwExtraInfo = IntPtr.Zero,
                },
            };

            // Right mouse button up
            inputs[1] = new MouseClickUtilities.Input
            {
                type = MouseClickUtilities.SendInputEventType.Mouse,
                mouseInput = new MouseClickUtilities.MouseInput
                {
                    dx = absoluteX,
                    dy = absoluteY,
                    mouseData = 0,
                    dwFlags = MouseClickUtilities.MouseEventFlags.Absolute | 
                              (rightClick ? MouseClickUtilities.MouseEventFlags.RightUp : MouseClickUtilities.MouseEventFlags.LeftUp),
                    time = 0,
                    dwExtraInfo = IntPtr.Zero,
                },
            };

            // Send the inputs
            MouseClickUtilities.SendInput((uint)inputs.Length, ref inputs[0], Marshal.SizeOf(inputs[0]));
        }
    }
}
