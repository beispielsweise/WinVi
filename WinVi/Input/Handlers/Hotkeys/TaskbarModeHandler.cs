using System;
using WinVi.UI;
using WinVi.UiAutomation.Taskbar;

namespace WinVi.Input.Handlers.Hotkeys
{
    /// <summary>
    /// A class responsible for TaskbarMode
    /// </summary>
    internal class TaskbarModeHandler : HotkeyBase
    {
        internal TaskbarModeHandler() : base() { }

        internal static void Execute()
        {
            try
            {
                Taskbar.GetTaskbarAppElements();
                Taskbar.GetTaskbarTrayElements();
            }
            catch (ArgumentNullException)
            {
                // Display CRITICAL ERROR MESSAGE
                Console.WriteLine("CRITICAL ERROR occured while trying to access Windows Taskbar");
            }

            OverlayWindow.GetCurrentOverlayInstance().Show(); // some func
        }
    }
}
