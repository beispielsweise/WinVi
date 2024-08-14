using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using WinVim.UI.Tray;

namespace WinVim
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        TrayManager _trayManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _trayManager = new TrayManager();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager.Dispose();

            base.OnExit(e);
        }
    }
}
