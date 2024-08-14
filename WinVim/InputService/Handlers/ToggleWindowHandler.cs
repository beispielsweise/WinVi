using System.Windows;

namespace WinVim.InputService.Handlers
{
    internal class ToggleWindowHandler
    {
        private readonly Window _window;
        
        public ToggleWindowHandler (Window window)
        {
            _window = window;
        }

        public void OnWindowToggle()
        {
            if (_window.IsVisible)
            {
                // Hide the window if it's currently visible
                _window.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show the window if it's currently hidden
                _window.Visibility = Visibility.Visible;
                _window.Top = 0; // Position at top-left corner
                _window.Left = 0;
                _window.Width = SystemParameters.PrimaryScreenWidth;
                _window.Height = SystemParameters.PrimaryScreenHeight;
            }
        }
    }
}
