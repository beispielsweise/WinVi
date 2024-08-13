using System;
using System.Windows;
using System.Windows.Media;
using WinVim.Input;

namespace WinVim.OverlayWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Input stuff
        HotkeyExample hotkeyExample;
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private bool _lPressed = false;

        public MainWindow()
        {
            InitializeComponent();
            OnCreateModifyWindow();
             
            hotkeyExample = new HotkeyExample(this);
        }

        private void OnCreateModifyWindow()
        {
            this.Title = "OverlayWindow";

            // This is what makes the window transparent and unclickable 
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            
            // Invisible by default, always on top
            this.Visibility = Visibility.Hidden;
            this.Topmost = true;

            // Not really needed, since the window is created invisible. Emergency thingy,
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        
        protected override void OnClosed(EventArgs e)
        {
            hotkeyExample.Unhook();
            base.OnClosed(e);
        }

    }
}