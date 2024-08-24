using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
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

            HintCanvas.Height = SystemParameters.PrimaryScreenHeight;
            HintCanvas.Width = SystemParameters.PrimaryScreenWidth;
        }

        internal void HideWindow()
        {
            this.Hide();
        }

        internal void ShowWindow()
        {
            this.Show(); 
        }

        internal void DrawHintCanvas(IReadOnlyDictionary<string, Rect> dict)
        {
            foreach (KeyValuePair<string, Rect> kvp in dict)
                CreateHintBlock(kvp.Key, kvp.Value);
        }

        private void CreateHintBlock(string text, System.Windows.Rect rect)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = 12,                      // Changable
                FontWeight = FontWeights.Bold,      // Changable
                Foreground = Brushes.Black,         // Changable
                // Margin = new Thickness(),        Changable property, how much distance between border and text
                HorizontalAlignment = HorizontalAlignment.Left,     
                VerticalAlignment = VerticalAlignment.Top,
            };

            Border border = new Border
            {
                Width = textBlock.Width,
                Background = new SolidColorBrush(Color.FromArgb(80, 255, 255, 0)),  // Yellow background with opacity, changable 
                BorderBrush = Brushes.Black,                                        // Changable
                BorderThickness = new Thickness(2),                                 // Changable
                CornerRadius = new CornerRadius(5),                                 // Changable
                Padding = new Thickness(2),                                         // Changable
                Child = textBlock
            };

            Canvas.SetLeft(border, rect.X);
            Canvas.SetTop(border, rect.Y);

            HintCanvas.Children.Add(border);
        }

        internal void ClearHintCanvas()
        {
            HintCanvas.Children.Clear();
        }
    }
}