﻿using System;
using System.Windows.Forms;

namespace WinVi.UI.Tray
{
    internal enum TrayIconStatus
    {
        Default, 
        CriticalError,
        InsertMode,
        OverlayOn
    }

    /// <summary>
    /// Tray manager, that is responsible for creating, managing and deleting system tray icon
    /// Uses singleton pattern
    /// </summary>
    internal class TrayManager : IDisposable
    {
        private static readonly Lazy<TrayManager> _instance = new Lazy<TrayManager>(() => new TrayManager());

        // Create tray instance
        private static NotifyIcon _trayIcon;

        private TrayManager()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.DefaultIcon,
                Text = "WinVi",
                Visible = true,
            };

            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add("Exit", OnExitTrayManager);

            _trayIcon.ContextMenu = contextMenu;
            // _trayIcon.DoubleClick += (sender, args) => ShowWindow();     // Accessing settings menu in the future

        }

        /// <summary>
        /// Gets an instance of a TrayManager
        /// </summary>
        public static TrayManager Instance => _instance.Value;

        /// <summary>
        /// Sets tray icon status 
        /// </summary>
        /// <param name="status">Corresponding resource status</param>
        /// <param name="text">Text to be displayed, optional</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetIconStatus(TrayIconStatus status, string text = "???")
        {
            switch (status)
            {
                case TrayIconStatus.Default:
                    _trayIcon.Icon = Properties.Resources.DefaultIcon;
                    _trayIcon.Text = string.IsNullOrEmpty(text) ? "WinVi" : text;
                    break;
                case TrayIconStatus.CriticalError:
                    _trayIcon.Icon = Properties.Resources.CriticalErrorIcon;
                    _trayIcon.Text = string.IsNullOrEmpty(text) ? "Critical error" : text;
                    break;
                case TrayIconStatus.InsertMode:
                    _trayIcon.Icon = Properties.Resources.InsertModeIcon;
                    _trayIcon.Text = string.IsNullOrEmpty(text) ? "Insert mode" : text;
                    break;
                case TrayIconStatus.OverlayOn:
                    _trayIcon.Icon = Properties.Resources.OverlayOnIcon;
                    _trayIcon.Text = string.IsNullOrEmpty(text) ? "Overlay on" : text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public void OnExitTrayManager(object sender, EventArgs e)
        {
            Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        public void Dispose()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Dispose();
                _trayIcon = null;
            }

            GC.SuppressFinalize(this);
        }

        // Optional finalizer if needed for cleanup
        ~TrayManager()
        {
            Dispose();
        }
    }
}