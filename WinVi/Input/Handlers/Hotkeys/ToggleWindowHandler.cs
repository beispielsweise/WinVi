using WinVi.Input.Handlers.Hotkeys;
using WinVi.UI;

namespace WinVi.Input.Handlers
{
    /// <summary>
    /// more of a test class, toggles the window on anf off
    /// </summary>
    internal class ToggleWindowHandler : HotkeyBase 
    {
                
        internal ToggleWindowHandler() : base() { }


        internal override void Execute()
        {
            if (_overlayWindow.IsVisible)
            {
                // Hide the window if it's currently visible
                _overlayWindow.CollapseWindow();
            }
            else
            {
                // Show the window if it's currently hidden
                _overlayWindow.ShowWindow();
            }
        }
    }
}
