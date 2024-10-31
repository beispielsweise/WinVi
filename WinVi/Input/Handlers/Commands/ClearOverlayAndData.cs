using WinVi.Input.Handlers.Modes;
using WinVi.UI;
using WinVi.UiAutomation;

namespace WinVi.Input.Handlers.Commands
{
    /// <summary>
    /// Force closes OverlayWindow
    /// </summary>
    internal class ClearOverlayAndData
    {
        internal static void Execute()
        {
            OverlayWindow.Instance.ClearHintCanvas();
            OverlayWindow.Instance.Hide();

            HintKeyProcessor.ResetCurrentSequence();
        }
    }
}
