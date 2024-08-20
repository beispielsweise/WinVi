using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using WinVi.Input;
using WinVi.UiAutomation.Taskbar;
using static System.Net.Mime.MediaTypeNames;

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

        internal void DrawTaskbarHintCanvas()
        {
            foreach (KeyValuePair<string, AutomationElement> kvp in Taskbar.Instance.AutomationElementsDict)
            {
                CreateTextBlock(kvp.Key, kvp.Value.Current.BoundingRectangle);
                Console.WriteLine("HERE");
            }
        }

        private void CreateTextBlock(string text, System.Windows.Rect rect)
        {
            Border border = new Border
            {
                Background = Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0)), // Half-transparent yellow
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(2),
            };
            
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = 16,
                Foreground = Brushes.Black,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            border.Child = textBlock;

            Canvas.SetLeft(border, rect.X);
            Canvas.SetTop(border, rect.Y + (rect.Height / 2));

            HintCanvas.Children.Add(border);
        }

        internal void ClearHintCanvas()
        {
            HintCanvas.Children.Clear();
        }
    }
}