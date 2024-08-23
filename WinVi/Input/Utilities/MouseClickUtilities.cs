using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WinVi.Input.ClickManager;

namespace WinVi.Input.Utilities
{
    internal class MouseClickUtilities
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, ref Input pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Input
        {
            public SendInputEventType type;
            public MouseInput mouseInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        internal enum SendInputEventType : uint
        {
            Mouse = 0,
            // Keyboard = 1,
            // Hardware = 2,
        }

        [Flags]
        internal enum MouseEventFlags : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            Absolute = 0x8000,
        }
    }
}
