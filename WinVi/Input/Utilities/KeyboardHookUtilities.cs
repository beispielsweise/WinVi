using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WinVi.Input.Utilities
{
    /// <summary>
    /// A class with external functions and constants for the input system
    /// </summary>
    internal class KeyboardHookUtilities
    {
        // Windows API functions
        // Native methods
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        // Delegate for the hook procedure
        internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        // public IntPtr _hookID = IntPtr.Zero; - needs to be included in each an every hook, because you know. Duh.

        internal enum KeyboardHooks : int
        {
            WH_KEYBOARD_LL = 13
        }

        internal enum KeyboardEventTypes : int
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SyskeyDown = 0x0104
        }


        internal static readonly IReadOnlyDictionary<int, string> _vkKeyCodes = new Dictionary<int, string>
        {
            { 27, "escape" },
            { 112, "f1" },
            { 113, "f2" },
            { 114, "f3" },
            { 115, "f4" },
            { 116, "f5" },
            { 117, "f6" },
            { 118, "f7" },
            { 119, "f8" },
            { 120, "f9" },
            { 121, "f10" },
            { 122, "f11" },
            { 123, "f12" },
            { 8, "back" },
            { 49, "d1" },
            { 50, "d2" },
            { 51, "d3" },
            { 52, "d4" },
            { 53, "d5" },
            { 54, "d6" },
            { 55, "d7" },
            { 56, "d8" },
            { 57, "d9" },
            { 48, "d0" },
            { 189, "oemminus" },
            { 187, "oemplus" },
            { 9, "tab" },
            { 81, "q" },
            { 87, "w" },
            { 69, "e" },
            { 82, "r" },
            { 84, "t" },
            { 89, "y" },
            { 85, "u" },
            { 73, "i" },
            { 79, "o" },
            { 80, "p" },
            { 219, "oemopenbrackets" },
            { 221, "oem6" },
            { 13, "return" },
            { 20, "capital" },
            { 65, "a" },
            { 83, "s" },
            { 68, "d" },
            { 70, "f" },
            { 71, "g" },
            { 72, "h" },
            { 74, "j" },
            { 75, "k" },
            { 76, "l" },
            { 186, "oem1" },
            { 222, "oem7" },
            { 220, "oem5" },
            { 160, "shift" },
            { 226, "oembackslash" },
            { 90, "z" },
            { 88, "x" },
            { 67, "c" },
            { 86, "v" },
            { 66, "b" },
            { 78, "n" },
            { 77, "m" },
            { 188, "oemcomma" },
            { 190, "oemperiod" },
            { 38, "up" },
            { 161, "rshift" },
            { 162, "ctrl" },
            { 91, "win" },
            { 164, "alt" },
            { 32, "space" },
            { 37, "left" },
            { 40, "down" },
            { 39, "right" }
        };
    }
}