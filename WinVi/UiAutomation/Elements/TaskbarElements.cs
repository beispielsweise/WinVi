﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI.Misc;

namespace WinVi.UiAutomation.Elements
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
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
        //private static readonly string _cnTaskbarSystemApps = "ToggleButton";
        private static readonly string _cnTaskbarUserApps = "Taskbar.TaskListButtonAutomationPeer";

        // Right side taskbar, tray icons
        private static readonly string _aidSystemTrayIcon = "SystemTrayIcon";
        private static readonly string _aidNotifyItemIcon = "NotifyItemIcon";

        // TODO: For config implementation, so that the last hint element does not go off screen
        private static bool _isShowDesktopButtonVisible = false;

        /// <summary>
        /// Get global taskbar AutomationElement
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Is thrown in case any elements are empty. This text is then displayed in tray</exception>
        private static AutomationElement GetGlobalTaskbar()
        {

            AutomationElement taskbar = AutomationElement.RootElement
                ?? throw new Exception("Root element not found");
            taskbar = taskbar.FindFirst(
                TreeScope.Children,
                new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName))
                ?? throw new Exception("SHELL_TrayWnd not found");

            taskbar = taskbar.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements))
                ?? throw new Exception("Windows.UI.Input.InputSite.WindowClass not found");

            return taskbar;
        }

        /// <summary>
        /// Fills _automationElementsDict with corresponding HintKeys and Positions
        /// </summary>
        /// <exception cref="Exception">Is thrown in case any elements are empty. This text is then displayed in tray</exception>
        internal static void GetTaskbarElements()
        {
            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new Exception("Global taskbar not found");

            AutomationElementDictionary.Instance.Reset();
            UIKeysGenerator.Instance.Reset();

            // Left side of the Taskbar
            AutomationElement lTaskbar = taskbar
                .FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new Exception("TaskbarFrame not found");
            // Windows and Search elements - bugged, no implementation, issue #8
            /*AutomationElementCollection taskbarSystemApps = lTaskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new Exception("ToggleButton system elements not found");*/
            // User-defined apps on the left side of the taskbar
            AutomationElementCollection taskbarUserApps = lTaskbar
                .FindAll(
                TreeScope.Children,
                new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new Exception("Taskbar.TaskListButtonAutomationPeer");
            AutomationElementDictionary.Instance.AddElements(taskbarUserApps, false);

            // Right side of the Taskbar
            // Apps in notification tray
            AutomationElementCollection rTaskbar = taskbar.FindAll(
                TreeScope.Children,
                new OrCondition(
                    new PropertyCondition(AutomationElement.AutomationIdProperty, _aidNotifyItemIcon),
                    new PropertyCondition(AutomationElement.AutomationIdProperty, _aidSystemTrayIcon)
                ));
            AutomationElementDictionary.Instance.AddElements(rTaskbar, _isShowDesktopButtonVisible);
        }
    }
}
