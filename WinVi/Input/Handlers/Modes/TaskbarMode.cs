using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using WinVi.UI;
using WinVi.UI.Tray;
using WinVi.UiAutomation.Taskbar;

namespace WinVi.Input.Handlers.Modes
{
    /// <summary>
    /// A class responsible for TaskbarMode
    /// </summary>
    internal class TaskbarMode
    {
        private static string _currentSequence = string.Empty;
        public enum HintKeyStatus
        {
            Pressed,
            Error,
            Skip
        }

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
