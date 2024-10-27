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
    internal class TaskbarElements : IDisposable
    {
        // Variables with properties, that help indicate different UI Elements, placed from top to bottom
        // _cn - ClassName
        // _aid - AutomationID 
        private static readonly Lazy<TaskbarElements> _instance = new Lazy<TaskbarElements>(() => new TaskbarElements(), true);

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

        internal static TaskbarElements Instance => _instance.Value;

        /// <summary>
        /// Access _automationElementDict instance 
        /// </summary>
        internal IReadOnlyDictionary<string, Rect> AutomationElementsDict => _automationElementsDict;

        /// <summary>
        /// Get global taskbar AutomationElement
        /// </summary>
        /// <returns></returns>
        private static AutomationElement GetGlobalTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));
            return taskbar;
        }

        /// <summary>
        /// Fills _automationElementsDict with corresponding HintKeys and Positions
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown in case some of the taskbar elements were empty</exception>
        internal void FillTaskbarElementsDict()
        {
            _automationElementsDict ??= new Dictionary<string, Rect>();   

            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new ArgumentNullException(nameof(taskbar));
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new ArgumentNullException(nameof(taskbar));

            UIKeysGenerator.Instance.Reset();
            /*
            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarSystemApps)
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);
            */
            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement element in taskbarUserApps)
                _automationElementsDict.Add(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);
        }

        internal void GetContextMenu()
        {
            // TODO:
            // The parent element (e.g. AA) should be always on display, but the child elements (Context Menu in this case, rename?)
            // should be displayed with it as well. How to detect a child element - that is a question for later
        }

        public void Dispose()
        {
            _automationElementsDict = null;
        }
    }
}
