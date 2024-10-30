using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WinVi.UiAutomation;

namespace WinVi.UI
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// WIth a "handicaped" singleton pattern
    /// </summary>
    partial class OverlayWindow : Window
    {
        private static OverlayWindow _instance;

        // Variables that customise how hint look. Placeholders, will be used in config system
        private static int _fontSize = 12;
        private static FontWeight _fontWeigt = FontWeights.Bold;      // Changable
        private static Brush _fontForeground = Brushes.Black;         // Changable
        private static Brush _borderBackground = new SolidColorBrush(Color.FromArgb(80, 255, 255, 0)); // Yellow background with opacity, changable 
        private static Brush _borderOutline = Brushes.Black;    // Changable
        private static int _borderThickness = 2;                // Changable
        private static int _borderCornerRadius = 5;             // Changable
        private static int _borderPadding = 2;                  // Changable

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
        
        // Redundant, replace
        internal void HideWindow()
        {
            this.Hide();
        }

        // Redundant, replace
        internal void ShowWindow()
        {
            this.Show(); 
        }

        /// <summary>
        /// Initializes the process of drawing keys to the overlay window 
        /// </summary>
        /// <param name="dict"></param>
        internal void DrawHintCanvas()
        {
            foreach (KeyValuePair<string, Rect> kvp in AutomationElementDictionary.Instance.GetDictionary())
                CreateHintBlock(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Creates a single Hint element
        /// </summary>
        /// <param name="text">Text to be displayed over the element</param>
        /// <param name="rect">Position of the element</param>
        private void CreateHintBlock(string text, System.Windows.Rect rect)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = _fontSize,                   
                FontWeight = _fontWeigt,      
                Foreground = _fontForeground,         
                // Margin = new Thickness(),        Changable property, how much distance between border and text ???
                HorizontalAlignment = HorizontalAlignment.Left,     
                VerticalAlignment = VerticalAlignment.Top,
            };

            Border border = new Border
            {
                Width = textBlock.Width,
                Background = _borderBackground, 
                BorderBrush = _borderOutline,                           
                BorderThickness = new Thickness(_borderThickness),          
                CornerRadius = new CornerRadius(_borderCornerRadius),          
                Padding = new Thickness(_borderPadding),                                         
                Child = textBlock
            };

            Canvas.SetLeft(border, rect.X);
            Canvas.SetTop(border, rect.Y - _fontSize * 2);

            HintCanvas.Children.Add(border);
        }

        /// <summary>
        /// Force clears the Hint Canvas
        /// </summary>
        internal void ClearHintCanvas()
        {
            HintCanvas.Children.Clear();
        }
    }
}