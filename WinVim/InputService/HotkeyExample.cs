﻿using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace WinVim.Input
{
    // Example class on how to use a hotkey. Custom-build system, CTRL + AHIFT + ALT + L
    internal class HotkeyExample
    {
        public IntPtr _hookID = IntPtr.Zero;
        
        // Button status fields
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private bool _lPressed = false;

        // Instance of a window to interract with 
        private readonly Window _window;

        public HotkeyExample(Window window)
        {
            this._window = window;

            SetHook();
        }
        
        // Setting the hook, so that windows sees it
        private void SetHook()
        {
            InputUtils.SetWindowsHookEx(InputUtils.WH_KEYBOARD_LL, HookCallback, InputUtils.GetModuleHandle(null), 0);
        }
    
        // When an event occures, this function is called
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // When key state is changed, we do stuff
            if (nCode >= 0 && (wParam == (IntPtr)InputUtils.WM_KEYDOWN || wParam == (IntPtr)InputUtils.WM_KEYUP))
            {
                // Read key pressed code
                int vkCode = Marshal.ReadInt32(lParam);
                bool isKeyDown = (wParam == (IntPtr)InputUtils.WM_KEYDOWN);

                // Hotkey mechanism logic
                switch (vkCode)
                {
                    case InputUtils.VK_LCONTROL:
                        _ctrlPressed = isKeyDown;
                        break;
                    case InputUtils.VK_LMENU:
                        _altPressed = isKeyDown;
                        break;
                    case InputUtils.VK_LSHIFT:
                        _shiftPressed = isKeyDown;
                        break;
                    case InputUtils.VK_L:
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

            return InputUtils.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
       
        // Self-explanatory
        public void Unhook()
        {
            if (_hookID != IntPtr.Zero)
            {
                InputUtils.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}
