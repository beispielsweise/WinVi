using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WinVi.Input.Managers.Utilities
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
        // public IntPtr _hookID = IntPtr.Zero; - needs to be included in each an every hook

        internal enum KeyboardHooks : int
        {
            WH_KEYBOARD_LL = 13
        }

        internal enum KeyboardEventTypes : int
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SyskeyDown = 0x0104,
            SyskeyUp = 0x0105
        }

        /// <summary>
        /// KeyCodes dicrionaty. Should be faster than using pre-built options
        /// </summary>
        internal static readonly IReadOnlyDictionary<int, string> _vkKeyCodes = new Dictionary<int, string>
        {
            { 27, "ESCAPE" },
            { 112, "F1" },
            { 113, "F2" },
            { 114, "F3" },
            { 115, "F4" },
            { 116, "F5" },
            { 117, "F6" },
            { 118, "F7" },
            { 119, "F8" },
            { 120, "F9" },
            { 121, "F10" },
            { 122, "F11" },
            { 123, "F12" },
            { 8, "BACK" },
            { 49, "D1" },
            { 50, "D2" },
            { 51, "D3" },
            { 52, "D4" },
            { 53, "D5" },
            { 54, "D6" },
            { 55, "D7" },
            { 56, "D8" },
            { 57, "D9" },
            { 48, "D0" },
            { 189, "OEMMINUS" },
            { 187, "OEMPLUS" },
            { 9, "TAB" },
            { 81, "Q" },
            { 87, "W" },
            { 69, "E" },
            { 82, "R" },
            { 84, "T" },
            { 89, "Y" },
            { 85, "U" },
            { 73, "I" },
            { 79, "O" },
            { 80, "P" },
            { 219, "OEMOPENBRACKETS" },
            { 221, "OEM6" },
            { 13, "RETURN" },
            { 20, "CAPITAL" },
            { 65, "A" },
            { 83, "S" },
            { 68, "D" },
            { 70, "F" },
            { 71, "G" },
            { 72, "H" },
            { 74, "J" },
            { 75, "K" },
            { 76, "L" },
            { 186, "OEM1" },
            { 222, "OEM7" },
            { 220, "OEM5" },
            { 160, "SHIFT" },
            { 226, "OEMBACKSLASH" },
            { 90, "Z" },
            { 88, "X" },
            { 67, "C" },
            { 86, "V" },
            { 66, "B" },
            { 78, "N" },
            { 77, "M" },
            { 188, "OEMCOMMA" },
            { 190, "OEMPERIOD" },
            { 38, "UP" },
            { 161, "RSHIFT" },
            { 162, "CTRL" },
            { 91, "WIN" },
            { 164, "ALT" },
            { 32, "SPACE" },
            { 37, "LEFT" },
            { 40, "DOWN" },
            { 39, "RIGHT" }
        };

        internal const string shiftKeyName = "SHIFT";
        internal const string ctrlKeyName = "CTRL";
        internal const string altKeyName = "ALT";

        internal const string escapeKeyName = "ESCAPE";
        internal const string insertModeKeyName = "I";
        internal const string taskbarModeKeyName = "T";
    }
}