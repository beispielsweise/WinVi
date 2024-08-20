using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using WinVi.UiAutomation.Utilities;

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
        private Dictionary<string, AutomationElement> _automationElementsDict = new Dictionary<string, AutomationElement>(); 

        internal static Taskbar Instance => _instance.Value;

        internal IReadOnlyDictionary<string, AutomationElement> AutomationElementsDict => _automationElementsDict;

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
            _automationElementsDict ??= new Dictionary<string, AutomationElement>();   

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
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element);

            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarUserApps)
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element);
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
