using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using WinVi.UI;
using WinVi.UI.Tray;
using WinVi.UiAutomation.Taskbar;

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
                TaskbarElements.Instance.FillTaskbarElementsDict();
            }
            catch (ArgumentNullException)
            {
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError, "Cannot find taskbar elements. Taskbar hidden?");
                return false;
            }

            OverlayWindow.Instance.DrawHintCanvas(TaskbarElements.Instance.AutomationElementsDict);
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
            _currentSequence += vkString;

            if (TaskbarElements.Instance.AutomationElementsDict.ContainsKey(_currentSequence))
            {
                if (TaskbarElements.Instance.AutomationElementsDict.TryGetValue(_currentSequence, out Rect rect))
                {
                    ClickManager.Instance.Click(rect, _shiftPressed);
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
