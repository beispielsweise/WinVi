using System;
using System.Windows;
using WinVi.Input;
using WinVi.UI.Tray;

namespace WinVi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
                _trayManager.SetIconStatus(TrayIconStatus.Error); 
                _hookManager = null; 
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager.Dispose();
            _hookManager.Dispose();
            base.OnExit(e);
        }

        internal HookManager GetHookManager()
        {
            return _hookManager;
        }
    }
}
