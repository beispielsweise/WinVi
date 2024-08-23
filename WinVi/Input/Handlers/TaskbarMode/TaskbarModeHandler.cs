using System;
using System.Windows.Automation;
using WinVi.UI;
using WinVi.UI.Tray;
using WinVi.UiAutomation.Taskbar;

namespace WinVi.Input.Handlers.TaskbarMode
{
    /// <summary>
    /// A class responsible for TaskbarMode
    /// </summary>
    internal class TaskbarModeHandler
    {
        private static string _currentSequence = string.Empty;

        internal static bool Execute()
        {
            try
            {
                Taskbar.Instance.FillTaskbarElementsDict();
            }
            catch (ArgumentNullException)
            {
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError, "Cannot find taskbar elements. Taskbar hidden?");
                return false;
            }

            OverlayWindow.Instance.DrawHintCanvas(Taskbar.Instance.AutomationElementsDict);
            OverlayWindow.Instance.Show();
            return true;
        }

        internal static bool ProcessHintKey(string vkString)
        {
            // Append the key to the current sequence
            _currentSequence += vkString;

            // Check if the current sequence matches any keys in the dictionary
            if (Taskbar.Instance.AutomationElementsDict.ContainsKey(_currentSequence))
            {

                // Reset the sequence after the action is executed
                _currentSequence = string.Empty;
                // A corresponding action needs to be triggered
                
                return true;
            }
            else if (_currentSequence.Length > 2)
            {
                // If the sequence is invalid, reset it
                _currentSequence = string.Empty;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
