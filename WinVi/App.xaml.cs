using System;
using System.Windows;
using WinVi.Input;
using WinVi.UI;
using WinVi.UI.Tray;

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

            // Force initialize the Overlay window Instance and force Collapse it
            // Prevents a bug, where the Overlay window opens only on 2nd hotkey press.
            OverlayWindow.Instance.CollapseWindow();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager?.Dispose();
            _hookManager?.Dispose();
            base.OnExit(e);
        }

        internal HookManager GetHookManagerInstance() => _hookManager;
        internal TrayManager GetTrayManagerInstance() => _trayManager;
    }
}
