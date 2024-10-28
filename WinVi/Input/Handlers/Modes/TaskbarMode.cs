using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using WinVi.Input.Utilities;
using WinVi.UI;
using WinVi.UI.Tray;
using WinVi.UiAutomation;
using WinVi.UiAutomation.Elements;

namespace WinVi.Input.Handlers.Modes
{
    /// <summary>
    /// A class responsible for processing TaskbarMode commands
    /// </summary>
    internal class TaskbarMode
    {
        private static string _currentSequence = string.Empty;
        // Return types, used to set TrayIcon status
        public enum HintKeyStatus
        {
            Pressed,
            Error,
            Skip
        }

        /// <summary>
        /// Processes initial overlay window opening
        /// </summary>
        /// <returns></returns>
        internal static bool OpenOverlay()
        {
            try
            {
                TaskbarElements.FillTaskbarElementsDict();
            }
            catch (Exception e)
            {
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError, e.Message);
                return false;
            }

            OverlayWindow.Instance.DrawHintCanvas();
            OverlayWindow.Instance.Show();
            return true;
        }

        /// <summary>
        /// Processes a hint key, that user pressed
        /// </summary>
        /// <param name="vkString">Key name</param>
        /// <param name="_shiftPressed">Is used to determine the right click, e.g. user pressing AA or aA or A</param>
        /// <returns>A status tat is passed to TrayIcon</returns>
        internal static HintKeyStatus ProcessHintKey(string vkString, bool _shiftPressed)
        {
            if (!vkString.Equals(KeyboardHookUtilities.shiftKeyName))
                _currentSequence += vkString;

            if (AutomationElementsDictionary.Instance.ContainsKey(_currentSequence))
            {
                if (AutomationElementsDictionary.Instance.TryGetValue(_currentSequence, out Rect rect))
                {
                    ClickManager.Instance.Click(rect, _shiftPressed, false);
                }

                _currentSequence = string.Empty;
                return HintKeyStatus.Pressed;
            }
            else if (_currentSequence.Length >= 2)
            {
                _currentSequence = string.Empty;
                return HintKeyStatus.Error;
            }
            else
            {
                return HintKeyStatus.Skip;
            }
        }
    }
}
