using System;
using System.Windows.Forms;

namespace WinVi.UI.Tray
{
    internal enum TrayIconStatus
    {
        Normal, 
        Error,
        InsertMode,
        OverlayOn
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
        private static NotifyIcon _trayIcon;

        private TrayManager()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.Normal,
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

        public static void SetIconStatus(TrayIconStatus status)
        {
            switch (status)
            {
                case TrayIconStatus.Normal:
                    _trayIcon.Icon = Properties.Resources.Normal;
                    _trayIcon.Text = "WinVi";
                    break;
                case TrayIconStatus.Error:
                    _trayIcon.Icon = Properties.Resources.CriticalError;
                    _trayIcon.Text = "Critical Error";
                    break;
                case TrayIconStatus.InsertMode:
                    _trayIcon.Icon = Properties.Resources.InsertMode;
                    _trayIcon.Text = "Insert mode";
                    break;
                case TrayIconStatus.OverlayOn:
                    _trayIcon.Icon = Properties.Resources.OverlayOn;
                    _trayIcon.Text = "Overlay is on";
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
