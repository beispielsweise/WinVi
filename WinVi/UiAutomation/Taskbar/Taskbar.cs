using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI.Misc;

namespace WinVi.UiAutomation.Taskbar
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
    /// TODO: Make 1 global function to get ALL taskbar elkements at once
    /// </summary>
    internal class Taskbar : IDisposable
    {
        // Variables with properties, that help indicate different UI Elements, placed from top to bottom
        // _cn - ClassName
        // _aid - AutomationID 
        private static readonly Lazy<Taskbar> _instance = new Lazy<Taskbar>(() => new Taskbar(), true);

        // Global taskbar
        private static readonly string _cnGlobalTaskbarName = "Shell_TrayWnd";
        private static readonly string _cnAllTaskbarElements = "Windows.UI.Input.InputSite.WindowClass";

        // Left side taskbar, system and user apps only
        private static readonly string _aidTaskbarAppsFrame = "TaskbarFrame";
        private static readonly string _cnTaskbarSystemApps = "ToggleButton";
        private static readonly string _cnTaskbarUserApps = "Taskbar.TaskListButtonAutomationPeer";

        // Right side taskbar, tray icons
        private static readonly string _aidSystemTrayIcon = "SystemTrayIcon";
        private static readonly string _aidNotifyItemIcon = "NotifyItemIcon";

        // A dictionary, that stores current taskbar elements
        private Dictionary<string, Rect> _automationElementsDict = new Dictionary<string, Rect>(); 

        internal static Taskbar Instance => _instance.Value;

        internal IReadOnlyDictionary<string, Rect> AutomationElementsDict => _automationElementsDict;

        private static AutomationElement GetGlobalTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));
            return taskbar;
        }

        internal void FillTaskbarElementsDict()
        {
            _automationElementsDict ??= new Dictionary<string, Rect>();   

            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new ArgumentNullException(nameof(taskbar));
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new ArgumentNullException(nameof(taskbar));

            UIKeysGenerator.Instance.Reset();
            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarSystemApps)
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);

            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarUserApps)
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);
        }

        internal void GetContextMenu()
        {
            // TODO: Implement the following idea:
            // For the main taskbar elements, we implement a key like 1AA, 1AS, 1AF and so on. So, 1 will be an identifier of the main layer.
            // HOWEVER, if we open a context menu, we do 2KK, 2KL or such. Then, if there is another layer, we add 3 and so on. This is a good idea in terms of memory-efficiency
        }

        public void InvokeDictElement(string key)
        {
            // Console.WriteLine(_automationElementsDict[key].Current.Name);
            
        }

        public void Dispose()
        {
            _automationElementsDict = null;
        }
    }
}
