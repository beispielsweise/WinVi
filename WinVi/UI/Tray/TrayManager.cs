using System;
using System.Windows.Forms;

namespace WinVi.UI.Tray
{
    internal enum TrayIconStatus
    {
        Normal, 
        Error,
    }

    /// <summary>
    /// Tray manager, that is responsible for creating, managing and deleting system tray icon
    /// Uses singleton pattern
    /// </summary>
    internal class TrayManager :IDisposable
    {
        private static TrayManager _instance;
        private static readonly object _instanceLock = new object();

        // Create tray instance
        private NotifyIcon _trayIcon;

        private TrayManager()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.NormalWimVi,
                Text = "WinVi",
                Visible = true,
            };

            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add("Exit", OnExitTrayManager);

            _trayIcon.ContextMenu = contextMenu;

            // _trayIcon.DoubleClick += (sender, args) => ShowWindow();

        }

        public static TrayManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new TrayManager();
                    return _instance;
                }
            }
        }

        public void SetIconStatus(TrayIconStatus status)
        {
            switch (status)
            {
                case TrayIconStatus.Normal:
                    _trayIcon.Icon = Properties.Resources.NormalWimVi;
                    _trayIcon.Text = "WinVi";
                    break;
                case TrayIconStatus.Error:
                    _trayIcon.Icon = Properties.Resources.ErrorWimVi;
                    _trayIcon.Text = "Error";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
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
