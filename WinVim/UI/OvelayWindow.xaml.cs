using System;
using System.Windows;
using System.Windows.Media;
using WinVim.InputService;

namespace WinVim.UI
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        private HookManager _hookManager;

        public OverlayWindow()
        {
            InitializeComponent();
            ModifyWindow();

            _hookManager = new HookManager(this);
        }


        /// <summary>
        /// Modifies initial window properties. Used instead of XAML 
        /// </summary>
        private void ModifyWindow()
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

        private void OnExit(object sender, EventArgs e)
        {
            _hookManager.Dispose();
        }
    }
}