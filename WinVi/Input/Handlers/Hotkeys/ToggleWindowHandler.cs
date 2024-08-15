using WinVi.UI;

namespace WinVi.Input.Handlers
{
    internal class ToggleWindowHandler
    {
        private readonly OverlayWindow _window;
        
        internal ToggleWindowHandler (OverlayWindow window)
        {
            _window = window;
        }

        internal void OnWindowToggle()
        {
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
