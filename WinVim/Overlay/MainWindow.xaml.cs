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
        
        public MainWindow()
        {
            InitializeComponent();
            OnCreateModifyWindow();
             
            // Add hotkey
            hotkeyExample = new HotkeyExample(this);
        }

        // Modify initial window properties. Useid instead of XAML 
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

            // Not really needed, since the window is created invisible. Emergency thingy, in case something hoes south
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        
        // Self-explanatory
        protected override void OnClosed(EventArgs e)
        {
            hotkeyExample.Unhook();
            base.OnClosed(e);
        }

    }
}