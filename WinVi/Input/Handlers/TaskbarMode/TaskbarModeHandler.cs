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
        internal static void Execute()
        {
            try
            {
                Taskbar.Instance.GetTaskbarAppElements();
            }
            catch (ArgumentNullException)
            {
                // Display CRITICAL ERROR MESSAGE
                Console.WriteLine("CRITICAL ERROR occured while trying to access Windows Taskbar");
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError);
            }

            OverlayWindow.Instance.DrawTaskbarHintCanvas();
            OverlayWindow.Instance.Show();
        }
    }
}
