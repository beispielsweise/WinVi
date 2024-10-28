using WinVi.UI;
using WinVi.UiAutomation;
using WinVi.UiAutomation.Elements;

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

            AutomationElementsDictionary.Instance.Dispose();
        }
    }
}
