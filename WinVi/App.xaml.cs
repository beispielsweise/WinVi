using System;
using System.Windows;
using WinVi.Input;
using WinVi.UI;
using WinVi.UI.Tray;
using WinVi.UiAutomation.Utilities;

namespace WinVi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private HookManager _hookManager;
        private TrayManager _trayManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            _trayManager = TrayManager.Instance;

            try
            {
                _hookManager = HookManager.Instance; 
            }
            catch 
            {
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError); 
                _hookManager = null; 
            }

            UIKeysGenerator _uiKeysGenerator = UIKeysGenerator.Instance;

            // Force initialize the Overlay window Instance and force Collapse it
            // Prevents a bug, where the Overlay window opens only on 2nd hotkey press.
            // ! ONLY A TEMPORARY SOLUTION, THIS DECREASES PERFORMANCE!
            OverlayWindow.Instance.HideWindow();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager?.Dispose();
            _hookManager?.Dispose();
            base.OnExit(e);
        }
    }
}
