using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WinVi.Input.Utilities;
using WinVi.UI;

namespace WinVi.Input
{
    /// <summary>
    /// A class responsible for performing clicks system-wide.
    /// Moves cursor to a desired position
    /// </summary>
    internal class ClickManager
    {

        private static readonly Lazy<ClickManager> _instance = new Lazy<ClickManager>(() => new ClickManager(), true);

        internal static ClickManager Instance = _instance.Value;
        // For optimisations sake
        private static MouseClickUtilities.Input[] _inputs;

        /// <summary>
        /// Preforms a right or left click
        /// </summary>
        /// <param name="rect">Rectangle with the position coordinates</param>
        /// <param name="invokeRightClick">If right click should be performed</param>
        /// <param bame="returnCursor">If the cursor should be returned to it's initial position, default = true</param>
        public void Click(Rect rect, bool invokeRightClick, bool returnCursor = true)
        {
            MouseClickUtilities.CursorPoint currentCursorPos = GetCurrentCursorPos();

            var downFlag = MouseClickUtilities.MouseEventFlags.Absolute | 
                (invokeRightClick ? MouseClickUtilities.MouseEventFlags.RightDown : MouseClickUtilities.MouseEventFlags.LeftDown);
            var upFlag = MouseClickUtilities.MouseEventFlags.Absolute |
                (invokeRightClick ? MouseClickUtilities.MouseEventFlags.RightUp : MouseClickUtilities.MouseEventFlags.LeftUp);

            _inputs = new[]
            {
                CreateMouseInput(0, 0, downFlag),
                CreateMouseInput(0, 0, upFlag)
            };

            MouseClickUtilities.SetCursorPos((int)(rect.X + rect.Width / 1.5), (int)(rect.Y + rect.Height / 1.5));  // rect.Width / 2 is used to click in the middle of an element. 
            MouseClickUtilities.SendInput((uint)_inputs.Length, _inputs, Marshal.SizeOf(typeof(MouseClickUtilities.Input)));
            if (returnCursor)
                MouseClickUtilities.SetCursorPos(currentCursorPos.X, currentCursorPos.Y);
        }

        /// <summary>
        /// Creates mouse input
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="flags">Flags needed</param>
        /// <param name="mouseScrollAmount">Mouse scrolling amount (unused)</param>
        /// <returns></returns>
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
       
        /// <summary>
        /// Gets current cursor position
        /// </summary>
        /// <returns>Returns current Cursor position</returns>
        private MouseClickUtilities.CursorPoint GetCurrentCursorPos()
        {
            if (MouseClickUtilities.GetCursorPos(out var cursorPoint))
            {
                return cursorPoint;
            }
            return new MouseClickUtilities.CursorPoint { X = -1, Y = -1};
        }
    }
}
