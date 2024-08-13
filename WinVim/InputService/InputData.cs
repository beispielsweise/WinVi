using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinVim.Input
{
    /**
     * A class with external functions and constants for the input system
     */
    internal class InputData
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

        // DLL import for registering and unregistering global hotkeys
        // Not used, since we use a custom hotkey logic
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Constants for hotkey modifiers 
        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;

        // Constants for keyboard buttons
        // For some reason, I cannot work with HEX values here. So they are replaced by according integers
        public const int VK_LCONTROL = 162;        // Left Ctrl 
        public const int VK_LMENU = 164;           // Left Alt
        public const int VK_LSHIFT = 160;          // Left shift
        public const int VK_T = 76; 
        public const int VK_L = 0x4C; 

        // Constants for the keyboard hook (states)
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;

        // Delegate for the hook procedure
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        // public IntPtr _hookID = IntPtr.Zero;
    }
}
