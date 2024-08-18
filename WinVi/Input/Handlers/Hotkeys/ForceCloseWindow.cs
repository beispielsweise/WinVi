using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinVi.UI;

namespace WinVi.Input.Handlers.Hotkeys
{
    /// <summary>
    /// Force closes OverlayWindow
    /// </summary>
    internal class ForceCloseWindow
    {
        internal static void Execute()
        {
            OverlayWindow.Instance.CollapseWindow();
        }
    }
}
