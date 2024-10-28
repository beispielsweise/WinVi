using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using WinVi.Input.Handlers.Commands;
using WinVi.Input.Handlers.Modes;
using WinVi.Input.Utilities;
using WinVi.UI.Tray;
using WinVi.UiAutomation;
using WinVi.UiAutomation.Elements;

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
        private readonly KeyboardHookUtilities.LowLevelKeyboardProc _proc;

        // 
        private bool _isInsertModeEnabled= false;
        private bool _isOverlayWindowOpened = false;

        // Modifier buttons status fields
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private static string vkString = "";

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
            IntPtr hookID = KeyboardHookUtilities.SetWindowsHookEx((int)KeyboardHookUtilities.KeyboardHooks.WH_KEYBOARD_LL, _proc, KeyboardHookUtilities.GetModuleHandle(null), 0);
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
            vkString = KeyboardHookUtilities._vkKeyCodes.TryGetValue(Marshal.ReadInt32(lParam), out string keyString) ? keyString : "null";

            if (wParam == (IntPtr)KeyboardHookUtilities.KeyboardEventTypes.KeyDown || wParam == (IntPtr)KeyboardHookUtilities.KeyboardEventTypes.SyskeyDown)
            {
                CheckHotkeyButtonPressed(vkString, true);

                if (!CheckHotkeyCombinationPressed() && !_isInsertModeEnabled)
                {
                    // If overlay window is opened, process only these keys
                    if (_isOverlayWindowOpened)
                    {
                        switch (vkString)
                        {
                            case KeyboardHookUtilities.escapeKeyName:
                                CloseOverlayWindow();
                                return (IntPtr)1;
                            default:
                                HandleHintKeypress(); 
                                return (IntPtr)1;
                        }
                    }
                }
                else if (CheckHotkeyCombinationPressed() && !_isOverlayWindowOpened)
                {
                    if (_isInsertModeEnabled)
                    {
                        if (vkString == KeyboardHookUtilities.escapeKeyName)
                        {
                            ExitInsertMode();
                            return (IntPtr)1;
                        }
                        return KeyboardHookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }

                    switch (vkString)
                    {
                        case KeyboardHookUtilities.taskbarModeKeyName:
                            EnterTaskbarMode();
                            return (IntPtr)1;
                        case KeyboardHookUtilities.insertModeKeyName:
                            EnterInsertMode();
                            return (IntPtr)1;
                        default:
                            return KeyboardHookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }
                }
            }
            else if (wParam == (IntPtr)KeyboardHookUtilities.KeyboardEventTypes.KeyUp || (wParam == (IntPtr)KeyboardHookUtilities.KeyboardEventTypes.SyskeyUp))
                CheckHotkeyButtonPressed(vkString, false);

            return KeyboardHookUtilities.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Opens OvelayWindow, sets icon statuses for TaskbarMode
        /// </summary>
        private void EnterTaskbarMode()
        {
            if (TaskbarMode.OpenOverlay())
            {
                _isOverlayWindowOpened = true;
                TrayManager.SetIconStatus(TrayIconStatus.OverlayOn);
            }
        }

        /// <summary>
        /// WHEN Overlay Window is opened, processes hint keys
        /// </summary>
        private void HandleHintKeypress()
        {
            switch (TaskbarMode.ProcessHintKey(vkString, CheckShiftModifierPressed()))
            {
                case TaskbarMode.HintKeyStatus.Pressed:
                    CloseOverlayWindow();
                    return;
                case TaskbarMode.HintKeyStatus.Error:
                    TrayManager.SetIconStatus(TrayIconStatus.Default, "Hint does not exist");
                    return;
                case TaskbarMode.HintKeyStatus.Skip:
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Prevents program from accessing keyboard presses
        /// </summary>
        private void EnterInsertMode()
        {
            _isInsertModeEnabled = true;
            TrayManager.SetIconStatus(TrayIconStatus.InsertMode);
        }

        /// <summary>
        /// Exits insert mode
        /// </summary>
        private void ExitInsertMode()
        {
            _isInsertModeEnabled = false;
            TrayManager.SetIconStatus(TrayIconStatus.Default);
        }

        /// <summary>
        /// Force closes overlay window
        /// </summary>
        private void CloseOverlayWindow()
        {
            _isOverlayWindowOpened = false;
            TaskbarMode.ResetCurrentSequence();
            AutomationElementsDictionary.Instance.Dispose() ;
            ForceCloseWindow.Execute();
            TrayManager.SetIconStatus(TrayIconStatus.Default);
        }

        /// <summary>
        /// Checks if buttons CTRL SHIFT and ALT are pressed at the same time. 
        /// This is a standart shortcut modifyer which is NOT intended to be changed
        /// </summary>
        /// <param name="vkString"> keyboard character pressed</param>
        /// <param name="isKeyDown">is key pressed down</param>
        private void CheckHotkeyButtonPressed(string vkString, bool isKeyDown)
        {
            // Hotkey mechanism logic
            switch (vkString)
            {
                case KeyboardHookUtilities.shiftKeyName:
                    _shiftPressed = isKeyDown;
                    break;
                case KeyboardHookUtilities.ctrlKeyName:
                    _ctrlPressed = isKeyDown;
                    break;
                case KeyboardHookUtilities.altKeyName:
                    _altPressed = isKeyDown;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check if main Hotkey combination is pressed
        /// </summary>
        /// <returns></returns>
        private bool CheckHotkeyCombinationPressed()
        {
            return _ctrlPressed && _altPressed && _shiftPressed;
        }

        /// <summary>
        /// Checks if the Shift modifier is pressed (without alt or ctrl)
        /// </summary>
        /// <returns></returns>
        private bool CheckShiftModifierPressed()
        {
            return _shiftPressed && !_altPressed && !_ctrlPressed;
        }

        /// <summary>
        /// Disposes of resources and nullifies the singleton instance
        /// </summary>
        public void Dispose()
        {
            if (_hookID != IntPtr.Zero)
            {
                KeyboardHookUtilities.UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
    }
}
