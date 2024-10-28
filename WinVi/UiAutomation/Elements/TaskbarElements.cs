using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI.Misc;

namespace WinVi.UiAutomation.Elements
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
    /// TODO: Make 1 global function to get ALL taskbar elkements at once
    /// </summary>
    internal class TaskbarElements   
    {
        // Variables with properties, that help indicate different UI Elements, placed from top to bottom
        // _cn - ClassName
        // _aid - AutomationID 
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

        /// <summary>
        /// Get global taskbar AutomationElement
        /// </summary>
        /// <returns></returns>
        private static AutomationElement GetGlobalTaskbar()
        {
            AutomationElement taskbar = AutomationElement.RootElement
                ?? throw new Exception("Root element not found");
            taskbar = taskbar.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName))
               ?? throw new Exception("SHELL_TrayWnd not found");
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements))
                ?? throw new Exception("Windows.UI.Input.InputSite.WindowClass not found");

            return taskbar;
        }

        /// <summary>
        /// Fills _automationElementsDict with corresponding HintKeys and Positions
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown in case some of the taskbar elements were empty</exception>
        internal static void FillTaskbarElementsDict()
        {
            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new Exception("Global taskbar not found");
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new Exception("TaskbarFrame not found");

            AutomationElementsDictionary.Instance.Clear();
            UIKeysGenerator.Instance.Reset();

            // Windows and Search elements
            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new Exception("ToggleButton system elements not found");
            foreach (AutomationElement element in taskbarSystemApps)
                AutomationElementsDictionary.Instance.AddElement(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);
            // All the other apps on the taskbar
            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new Exception("Taskbar.TaskListButtonAutomationPeer");
            foreach (AutomationElement element in taskbarUserApps)
                AutomationElementsDictionary.Instance.AddElement(UIKeysGenerator.Instance.GetNextKey(), element.Current.BoundingRectangle);
            // Apps in notification tray
            // ---
        }

        internal static void GetElementContextMenu()
        {
            // TODO:
            // The parent element (e.g. AA) should be always on display, but the child elements (Context Menu in this case, rename?)
            // should be displayed with it as well. How to detect a child element - that is a question for later
        }
    }
}
