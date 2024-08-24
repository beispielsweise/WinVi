using WinVi.UI;
using WinVi.UiAutomation.Taskbar;

namespace WinVi.Input.Handlers.Commands
{
    /// <summary>
    /// Force closes OverlayWindow
    /// </summary>
    internal class ForceCloseWindow
    {
        internal static void Execute()
        {
            OverlayWindow.Instance.ClearHintCanvas();
            OverlayWindow.Instance.HideWindow();

            TaskbarElements.Instance.Dispose();
        }
    }
}
