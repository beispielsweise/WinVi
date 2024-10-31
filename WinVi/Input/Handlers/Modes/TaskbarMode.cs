using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Windows;
using WinVi.Input.Managers.Utilities;
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
        /// <summary>
        /// Processes initial overlay window opening for TaskbarMode
        /// </summary>
        /// <returns></returns>
        internal static bool OpenOverlay()
        {
            try
            {
                TaskbarElements.GetTaskbarElements();
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
    }
}
