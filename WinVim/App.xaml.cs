using System.Windows;
using WinVim.Input;
using WinVim.UI.Tray;

namespace WinVim
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

            _hookManager = HookManager.Instance;
            _trayManager = TrayManager.Instance;
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
