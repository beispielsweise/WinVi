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
        internal static bool Execute()
        {
            try
            {
                Taskbar.Instance.FillTaskbarElementsDict();
            }
            catch (ArgumentNullException)
            {
                // Display CRITICAL ERROR MESSAGE
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError, "Cannot find taskbar elements. Taskbar hidden?");
                return false;
            }

            OverlayWindow.Instance.DrawHintCanvas(Taskbar.Instance.AutomationElementsDict);
            OverlayWindow.Instance.Show();
            return true;
        }

        internal static bool TryInvokeHint(string vkString)
        {

            return false;
        }
    }
}
