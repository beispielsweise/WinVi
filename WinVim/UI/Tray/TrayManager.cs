using System;
using System.Windows.Forms;

namespace WinVim.UI.Tray
{
    /// <summary>
    /// Tray manager, that is responsible for creating, managing and deleting system tray icon
    /// </summary>
    internal class TrayManager :IDisposable
    {
        // Create tray instance
        private NotifyIcon _trayIcon;

        public TrayManager()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.TrayIcon,
                Text = "My WPF TrayManager App",
                Visible = true,
            };

            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add("Exit", OnExitTrayManager);

            _trayIcon.ContextMenu = contextMenu;

            // _trayIcon.DoubleClick += (sender, args) => ShowWindow();

        }
        
        public void OnExitTrayManager(object sender, EventArgs e)
        {
            _trayIcon?.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        public void Dispose()
        {
            _trayIcon = null;    
        }
    }
}
