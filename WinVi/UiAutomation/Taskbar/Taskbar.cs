﻿using System;
using System.Windows.Automation;

namespace WinVi.UiAutomation.Taskbar
{
    /// <summary>
    /// A class, that is responsible for finding and interracting with elements in Taskbar (Taskbar and Tray)
    /// </summary>
    /// TODO: Add throwing exeptions everywhere, checks for Null
    internal class Taskbar : IDisposable
    {
        // Variables with properties, that help indicate different UI Elements, placed from top to bottom
        // _cn - ClassName
        // _aid - AutomationID 


        // TODO: Re-wirte using a Singleton pattern, adding a global array of automation elements available (maybe add an abstract class or interface in the future)


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


        private static AutomationElement GetGlobalTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnGlobalTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));
            return taskbar;
        }

        internal static AutomationElement[] GetTaskbarAppElements()
        {
            AutomationElement taskbar = GetGlobalTaskbar()
                ?? throw new ArgumentNullException(nameof(taskbar));
            
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarAppsFrame))
                ?? throw new ArgumentNullException(nameof(taskbar));
 

            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps))
                ?? throw new ArgumentNullException(nameof(taskbar));
            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps))
                ?? throw new ArgumentNullException(nameof(taskbar));

            AutomationElement[] appElementsArray = new AutomationElement[taskbarSystemApps.Count + taskbarUserApps.Count];

            taskbarSystemApps.CopyTo(appElementsArray, 0);
            taskbarUserApps.CopyTo(appElementsArray, taskbarSystemApps.Count);

            return appElementsArray;
        }

        internal static void GetTaskbarTrayElements()
        {
            //
        }

        public static void Invokelement(AutomationElement el)
        {
            
        }

        public void Dispose()
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
