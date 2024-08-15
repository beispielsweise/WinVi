﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Windows.Automation;

namespace WinVim.UiAutomation.Taskbar
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
    /// </summary>
    /// TODO: Add throwing exeptions everywhere, checks for Null
    internal class Taskbar
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

        private static AutomationElement GetTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));

            return taskbar;
        }


        internal static void GetTaskbarAppElements()
        {
            AutomationElement taskbar = GetTaskbar()
                ?? throw new ArgumentNullException(nameof(taskbar));
            
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new ArgumentNullException(nameof(taskbar));
 

            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement app in taskbarSystemApps)
            {
                Console.WriteLine(app.Current.Name + "\n" +  
                    app.Current.BoundingRectangle.ToString());
            }

            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            foreach (AutomationElement app in taskbarUserApps)
            {
                Console.WriteLine(app.Current.Name + "\n" +  
                    app.Current.BoundingRectangle.ToString());
            }

        }

        internal static void GetTaskbarTrayElements()
        {
            //
        }

        public static void ClickTaskbarAppElement()
        {

        }

        public static void ClickTaskbarTrayElement()
        {

        }


        /* DEBUG FUNCTION, DELETE!!! */
        private static void LogSubelements(AutomationElement element, bool more)
        {
            if (element == null)
            {
                Console.WriteLine("Element is null.");
                return;
            }

            Console.WriteLine("Element: " + element.Current.Name);

            string name = "";
            AutomationElementCollection children;
            // Find all children of the element
            if (more)
            {
                children = element.FindAll(TreeScope.Descendants, Condition.TrueCondition);
                name = "Descendant";
            }
            else
            {
                
                children = element.FindAll(TreeScope.Children, Condition.TrueCondition);
                name = "Child";
            }

            foreach (AutomationElement child in children)
            {
                Console.WriteLine(name + " Element: " + child.Current.Name);
                Console.WriteLine(" - Control Type: " + child.Current.ControlType.ProgrammaticName);
                Console.WriteLine(" - Automation Id: " + child.Current.AutomationId);
                Console.WriteLine(" - Class Name: " + child.Current.ClassName);
            }

            Console.WriteLine("_______________________________");
        }
    }
}