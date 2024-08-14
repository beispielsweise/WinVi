using System;
using System.Runtime.InteropServices;
using System.Windows;
using WinVim.Input;
using WinVim.InputService.Handlers;

namespace WinVim.InputService
{
    /// <summary>
    /// A class responsible for getting system-wide keyboard presses. 
    /// Contains logic for Processing keypresses
    /// </summary>
    internal class HookManager : IDisposable
    {
        private IntPtr _hookID = IntPtr.Zero;
        private readonly InputUtils.LowLevelKeyboardProc _proc;    
        // Button status fields
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private bool _lPressed = false;

        private ToggleWindowHandler _toggleWindowHandler;

        /// <summary>
        /// Initialize keyboard hook and handlers 
        /// </summary>
        /// <param name="window">Instance of a window, needed to </param>
        public HookManager(Window window) 
        {
            _proc = HookCallback;
            SetHook();
            
            // Initialize Handlers
            _toggleWindowHandler = new ToggleWindowHandler(window);
        }

        // Setting the hook, so that windows sees it
        private void SetHook()
        {
           InputUtils.SetWindowsHookEx(InputUtils.WH_KEYBOARD_LL, _proc, InputUtils.GetModuleHandle(null), 0);
        }

        // When an event occures, this function is called
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // When key state is changed, we do stuff
            if (nCode >= 0 && (wParam == (IntPtr)InputUtils.WM_KEYDOWN || wParam == (IntPtr)InputUtils.WM_KEYUP))
            {
                Console.WriteLine("Key pressed");
                // Read key pressed code
                int vkCode = Marshal.ReadInt32(lParam);
                bool isKeyDown = (wParam == (IntPtr)InputUtils.WM_KEYDOWN);

                string currentCombination = GetCurrentCombination(vkCode, isKeyDown);
            }

            return InputUtils.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private string GetCurrentCombination(int vkCode, bool isKeyDown)
        {
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
                _toggleWindowHandler.OnWindowToggle();
            }
            return null;
        }

        public void Dispose()
        {
            if (_hookID != IntPtr.Zero)
            {
                InputUtils.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}
