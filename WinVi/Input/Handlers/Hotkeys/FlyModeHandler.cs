using System;
using WinVi.UI;
using WinVi.UiAutomation.Taskbar;

namespace WinVi.Input.Handlers.Hotkeys
{
    internal class FlyModeHandler
    {
        private readonly OverlayWindow _window;
        
        internal FlyModeHandler(OverlayWindow window)
        {
            _window = window;
        }

        internal void OnWindowToggle()
        {
            try
            {
                Taskbar.GetTaskbarAppElements();
            }
            catch (ArgumentNullException) 
            {
                // Display CRITICAL ERROR MESSAGE
                Console.WriteLine("CRITICAL ERROR occured while trying to access Windows Taskbar");
            }

            if (_window.IsVisible)
            {
                // Hide the window if it's currently visible
                _window.CollapseWindow();
            }
            else
            {
                // Show the window if it's currently hidden
                _window.ShowWindow();    
            }
        }
    }
}
