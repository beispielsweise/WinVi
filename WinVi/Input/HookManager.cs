using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinVi.Input.Handlers.Hotkeys;
using WinVi.Input.Handlers.TaskbarMode;
using WinVi.UI.Tray;

namespace WinVi.Input
{
    /// <summary>
    /// A class responsible for getting system-wide keyboard presses. 
    /// Contains logic for Processing keypresses
    /// Uses singleton pattern
    /// </summary>
    internal class HookManager : IDisposable 
    {
        private static readonly Lazy<HookManager> _instance = new Lazy<HookManager>(() => new HookManager(), true);

        private IntPtr _hookID;
        private readonly HookUtilities.LowLevelKeyboardProc _proc;

        // 
        private bool _isInsertModeEnabled= false;
        private bool _isOverlayWindowOpened = false;

        // Modifier buttons status fields
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        
        /// <summary>
        /// Initialize keyboard hook and handlers 
        /// </summary>
        /// <param name="window">Instance of a window, needed to work with the overlay window itself </param>
        private HookManager()
        {
            _proc = HookCallback;
            try
            {
                _hookID = SetHook();
            }
            catch
            {
                throw new ArgumentNullException();
            }
        }

        internal static HookManager Instance => _instance.Value;

        /// <summary>
        /// Sets a keyboard hook
        /// </summary>
        private IntPtr SetHook()
        {
            IntPtr hookID = HookUtilities.SetWindowsHookEx(HookUtilities.WH_KEYBOARD_LL, _proc, HookUtilities.GetModuleHandle(null), 0);

            if (hookID == IntPtr.Zero)
                throw new ArgumentNullException();

            return hookID;
        }

        /// <summary>
        /// Runs when a keyboard event occures
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns>IntPtr(1): if the key is handeled by the program
        /// IntPtr: KeyboardUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam) - pass the handling to other programs
        /// </returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            string vkString = ConvertKeyCodeToString(
                Marshal.ReadInt32(lParam
                ));

            if (wParam == (IntPtr)HookUtilities.WM_KEYDOWN || wParam == (IntPtr)HookUtilities.WM_SYSKEYDOWN)
            {
                CheckHotkeyButtonPressed(vkString, true);

                // If the hotkey combintation is not pressed AND insert mode is not active
                // Process single-press buttons
                if (!CheckHotkeyCombinationPressed() && !_isInsertModeEnabled)
                {
                    // If overlay window is opened, process only these keys
                    if (_isOverlayWindowOpened)
                    {
                        // shift modifier?
                        switch (vkString)
                        {
                            case "escape":
                                _isOverlayWindowOpened = false;
                                ForceCloseWindow.Execute();
                                TrayManager.SetIconStatus(TrayIconStatus.Default);
                                return (IntPtr)1;
                            default:
                                return HookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                        }
                    }
                }
                // If the hotkey combination is pressed
                else if (CheckHotkeyCombinationPressed())
                {
                    // Logic for exiting the insert mode, ESC key
                    if (_isInsertModeEnabled)
                    {
                        if (vkString == "escape")
                        {
                            _isInsertModeEnabled = false;
                            TrayManager.SetIconStatus(TrayIconStatus.Default);
                            return (IntPtr)1;
                        }

                        return HookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }

                    // Process hotkeys
                    switch (vkString)
                    {
                        /*case "l":
                            _toggleWindowHandler.Execute();
                            return (IntPtr)1;*/
                        case "t":
                            _isOverlayWindowOpened = true;
                            TrayManager.SetIconStatus(TrayIconStatus.OverlayOn);
                            TaskbarModeHandler.Execute();
                            return (IntPtr)1;
                        case "i":
                            _isInsertModeEnabled = true;
                            TrayManager.SetIconStatus(TrayIconStatus.InsertMode);
                            return (IntPtr)1;
                        default:
                            return HookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }
                }
            }
            else if (wParam == (IntPtr)HookUtilities.WM_KEYUP)
            {
                CheckHotkeyButtonPressed(vkString, false);
            }

            return HookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Converts vkCode into its string value
        /// </summary>
        /// <param name="vkCode">initial integer code value</param>
        /// <returns>string: corresponding string value</returns>
        internal static string ConvertKeyCodeToString(int vkCode)
        {
            return ((Keys)vkCode).
                ToString().
                ToLower();
        }

        /// <summary>
        /// Checks if buttons CTRL SHIFT and ALT are pressed at the same time. 
        /// This is a standart shortcut modifyer which is NOT intended to be changed.
        /// </summary>
        /// <param name="vkString"> string: with a lowercase character</param>
        /// <param name="isKeyDown">bool: is key pressed</param>
        private void CheckHotkeyButtonPressed(string vkString, bool isKeyDown)
        {
            // Hotkey mechanism logic
            switch (vkString)
            {
                case "rshiftkey":
                case "lshiftkey":
                    _shiftPressed = isKeyDown;
                    break;
                case "rcontrolkey":
                case "lcontrolkey":
                    _ctrlPressed = isKeyDown;
                    break;
                case "rmenu":
                case "lmenu":
                    _altPressed = isKeyDown;
                    break;
                default:
                    break;
            }
        }

        private bool CheckHotkeyCombinationPressed()
        {
            return _ctrlPressed && _altPressed && _shiftPressed;
        }

        /// <summary>
        /// Disposes of resources and nullifies the singleton instance
        /// </summary>
        public void Dispose()
        {
            if (_hookID != IntPtr.Zero)
            {
                HookUtilities.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}



// USELESS STUFF

                // This is THE STUPID. 
                // So this is only for usual keys, no F1-F12 are allowed, or tabs or anything like that. 
                // That is because we are working with windows here and those buttons are crucial for the app to work. 
                // So, to ensure we are working only with usual keys, we are:
                // Checking if length is 1, then it is just a key like "l" or "k" etc
                // Checking if there is "oem" included. This is just a keyboard shenanigan for a couple of characters, like comma or dot
                // Checking if there is "d" followed by a number. This is by far the most idiotic one, while numbers 0-9 on a keyboard are labled as d0-d9
                //      The same would go for F1-F12 keys, but we are not using them :)
                //      So, to ensure that we are not missing out on any of em we are using a regex expression.
                // ! THIS CAN BE OPTIMISED (so maybe fix it later)
                //if (vkString.Length == 1 || vkString.Contains("oem") || Regex.IsMatch(vkString, @"d\d+"))
                //{
                //}

