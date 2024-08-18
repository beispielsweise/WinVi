using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;

namespace WinVi.UiAutomation.Taskbar
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
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
        private Dictionary<int, AutomationElement> _automationElementsDict = new Dictionary<int, AutomationElement>(); 

        internal static Taskbar Instance => _instance.Value;

        internal IReadOnlyDictionary<int, AutomationElement> AutomationElementsDict => _automationElementsDict;

        private static AutomationElement GetGlobalTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));
            return taskbar;
        }

        internal static void GetTaskbarAppElements()
        {
            Instance._automationElementsDict ??= new Dictionary<int, AutomationElement>();   

            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new ArgumentNullException(nameof(taskbar));
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new ArgumentNullException(nameof(taskbar));

            int uniqueID = Instance._automationElementsDict.Count;

            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarSystemApps)
            {
                Instance._automationElementsDict.Add(uniqueID, element);
                uniqueID++;
            }

            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarUserApps)
            {
                Instance._automationElementsDict.Add(uniqueID, element);
                uniqueID++;
            }
        }

        internal static void GetTaskbarTrayElements()
        {
            //
        }

        public void InvokeDictElement()
        {
            Console.WriteLine(_automationElementsDict[1].Current.Name);
        }

        public void Dispose()
        {
            _automationElementsDict = null;
        }
    }
}
