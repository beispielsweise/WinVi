using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinVi.Input.Handlers;
using System.Text.RegularExpressions;
using WinVi.Input.Handlers.Hotkeys;

namespace WinVi.Input
{
    /// <summary>
    /// A class responsible for getting system-wide keyboard presses. 
    /// Contains logic for Processing keypresses
    /// Uses singleton pattern
    /// </summary>
    internal class HookManager : IDisposable 
    {
        private static HookManager _instance;
        private static readonly object _instanceLock = new object();

        private IntPtr _hookID;
        private readonly KeyboardUtilities.LowLevelKeyboardProc _proc;

        // 
        private bool internalKeysDisabled = false;

        // Modifier buttons status fields
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        
        private ToggleWindowHandler _toggleWindowHandler;
        private TaskbarModeHandler _taskbarModeHandler;
        private ForceCloseWindow _forceCloseWindow;

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

        public static HookManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new HookManager();
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Initializes handlers, that need a specific window to operate (changing the window parameters, values etc.)
        /// Used in a WPF class, that needs to use these handlers
        /// </summary>
        /// <param name="window">Window: an instance of a window</param>
        internal void InitializeHotkeyHandlers()
        {
            // Initialize Handlers
            _toggleWindowHandler = new ToggleWindowHandler();
            _taskbarModeHandler = new TaskbarModeHandler();
            _forceCloseWindow = new ForceCloseWindow();
        }
        
        /// <summary>
        /// Sets a keyboard hook
        /// </summary>
        private IntPtr SetHook()
        {
            IntPtr hookID = KeyboardUtilities.SetWindowsHookEx(KeyboardUtilities.WH_KEYBOARD_LL, _proc, KeyboardUtilities.GetModuleHandle(null), 0);
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
        /// <returns>IntPtr</returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Read key pressed code and parse it to string
            string vkString = ConvertKeyCodeToString(
                Marshal.ReadInt32(lParam
                ));

            // Insure that the program is allowed to read keyboard inputs 
            if (internalKeysDisabled)
                return KeyboardUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);

            if (wParam == (IntPtr)KeyboardUtilities.WM_KEYDOWN || wParam == (IntPtr)KeyboardUtilities.WM_SYSKEYDOWN)
            {
                IsHotkeyCombinationPressed(vkString, true);

                // Check if the desired hotkey combination is pressed
                if (_ctrlPressed && _altPressed && _shiftPressed)
                {
                    switch (vkString)
                    {
                        case "l":
                            _toggleWindowHandler.Execute();
                            break;
                        case "t":
                            _taskbarModeHandler.Execute();
                            break;
                        case "q":
                            _forceCloseWindow.Execute();
                            break;
                        default:
                            break;
                    }

                    return KeyboardUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                }
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
                if (vkString.Length == 1 || vkString.Contains("oem") || Regex.IsMatch(vkString, @"d\d+"))
                {
                }
            }
            else if (wParam == (IntPtr)KeyboardUtilities.WM_KEYUP)
            {
                IsHotkeyCombinationPressed(vkString, false);
            }

            return KeyboardUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
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
        private void IsHotkeyCombinationPressed(string vkString, bool isKeyDown)
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

        public void Dispose()
        {
            if (_hookID != IntPtr.Zero)
            {
                KeyboardUtilities.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}
