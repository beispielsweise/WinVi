using System;
using System.Runtime.InteropServices;
using System.Windows;
using WinVim.Input;

namespace WinVim.Input
{
    internal class HotkeyExample
    {
        public IntPtr _hookID = IntPtr.Zero;

        // Input stuff
        InputData _inputData;

        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private bool _lPressed = false;

        private Window _window;

        public HotkeyExample(Window window)
        {
            this._window = window;

            _inputData = new InputData();
            SetHook();
        }

        private void SetHook()
        {
            InputData.SetWindowsHookEx(InputData.WH_KEYBOARD_LL, HookCallback, InputData.GetModuleHandle(null), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)InputData.WM_KEYDOWN || wParam == (IntPtr)InputData.WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool isKeyDown = (wParam == (IntPtr)InputData.WM_KEYDOWN);

                switch (vkCode)
                {
                    case InputData.VK_LCONTROL:
                        _ctrlPressed = isKeyDown;
                        break;
                    case InputData.VK_LMENU:
                        _altPressed = isKeyDown;
                        break;
                    case InputData.VK_LSHIFT:
                        _shiftPressed = isKeyDown;
                        break;
                    case InputData.VK_L:
                        _lPressed = isKeyDown;
                        break;
                }

                // Check if the desired hotkey combination is pressed
                if (_ctrlPressed && _altPressed && _shiftPressed && _lPressed)
                {
                    if (_window.IsVisible)
                    { 
                   
                        // Hide the window if it's currently visible
                        _window.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        // Show the window if it's currently hidden
                        _window.Visibility = Visibility.Visible;
                        _window.Top = 0; // Position at top-left corner
                        _window.Left = 0;
                        _window.Width = SystemParameters.PrimaryScreenWidth;
                        _window.Height = SystemParameters.PrimaryScreenHeight;
                    }

                }
            }

            return InputData.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        
        public void Unhook()
        {
            if (_hookID != IntPtr.Zero)
            {
                InputData.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}
