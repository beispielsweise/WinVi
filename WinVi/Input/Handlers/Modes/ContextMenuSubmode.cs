using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI;
using WinVi.UI.Misc;
using WinVi.UI.Tray;
using WinVi.UiAutomation;
using WinVi.UiAutomation.Elements;

namespace WinVi.Input.Handlers.Modes
{
    /// <summary>
    /// A class responsible for searching for and interracting with Context elements (MenutItemControlTypeID). 
    /// Universal for taskbar WindowShell elements and normal MenuItems.
    /// The program logic desides, when to open this mode.
    /// </summary>
    internal class ContextMenuSubmode
    {
        private static bool _isContextMenuVisible = false;

        internal static bool GetContextMenuStatus() => _isContextMenuVisible;

        internal static bool OpenOverlay(string hint)
        {
            if (ContextMenuElements.GetContextMenu(hint))
            {
                // TODO: Debug this, elements disappear
                _isContextMenuVisible = true;
                OverlayWindow.Instance.DrawHintCanvas(true);
                OverlayWindow.Instance.Show();
                return true;
            }

            _isContextMenuVisible = false;
            TrayManager.SetIconStatus(TrayIconStatus.Default, "No context menu detected");
            return false;
        }
    }
}
