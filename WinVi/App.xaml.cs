using System.Windows;
using WinVi.Input;
using WinVi.UI;
using WinVi.UI.Misc;
using WinVi.UI.Tray;

namespace WinVi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _ = TrayManager.Instance;
            try
            {
                _= HookManager.Instance; 
            }
            catch 
            {
                TrayManager.SetIconStatus(TrayIconStatus.CriticalError); 
            }
            _ = UIKeysGenerator.Instance;
            _ = ClickManager.Instance;
            // Force initialize the Overlay window Instance
            OverlayWindow.Instance.HideWindow();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TrayManager.Instance.Dispose();
            HookManager.Instance.Dispose();
            base.OnExit(e);
        }
    }
}
