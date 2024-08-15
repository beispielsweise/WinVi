using System;
using System.Runtime.InteropServices;

namespace WinVim.Input
{
    /// <summary>
    /// A class with external functions and constants for the input system
    /// </summary>
    internal class KeyboardUtilities
    {
        // Windows API functions
        // DLL import for getting keypresses
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        // Constants for the keyboard hook (states)
        internal const int WH_KEYBOARD_LL = 13;
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_SYSKEYDOWN = 0x0104;

        // Delegate for the hook procedure
        internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        // public IntPtr _hookID = IntPtr.Zero; - needs to be included in each an every hook, because you know. Duh.
    }
}
