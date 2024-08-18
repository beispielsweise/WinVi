using System;
using System.ComponentModel;
using System.Windows;
using WinVi.Input;

namespace WinVi.UI
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// WIth a "handicaped" singleton pattern
    /// </summary>
    partial class OverlayWindow : Window
    {
        private static OverlayWindow _instance;

        public OverlayWindow()
        {
            InitializeComponent();
            
            ModifyWindow();
        }

        internal static OverlayWindow Instance
        {
            get
            {
                _instance ??= new OverlayWindow();
                return _instance;
            }
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
            this.Background = System.Windows.Media.Brushes.Transparent;

            // Invisible by default, always on top
            this.Visibility = Visibility.Hidden;
            this.Topmost = true;

            // Not really needed, since the window is created invisible. Emergency thingy, in case something hoes south
            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void HideWindow()
        {
            this.Hide();
        }

        internal void ShowWindow()
        {
            this.Show(); 
        }
    }
}