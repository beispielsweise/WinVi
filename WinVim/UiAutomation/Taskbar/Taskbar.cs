using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace WinVim.UiAutomation.Taskbar
{
    internal class Taskbar
    {
        // Variables with properties, that help indicate different UI Elements 
        // ClassName
        private static readonly string _cnTaskbarName = "Shell_TrayWnd";
        private static readonly string _cnAllTaskbarElements = "Windows.UI.Input.InputSite.WindowClass";
        private static readonly string _cnTaskbarSystemApps = "ToggleButton";
        private static readonly string _cnTaskbarUserApps = "Taskbar.TaskListButtonAutomationPeer";

        // AutomationID 
        private static readonly string _aidTaskbarFrame = "TaskbarFrame";
        private static readonly string _aidSystemTrayIcon = "SystemTrayIcon";
        private static readonly string _aidNotifyItemIcon = "NotifyItemIcon";

        private static AutomationElement GetTaskbar()
        {
            AutomationElement taskbar = AutomationElement
                .RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarName));
            taskbar = taskbar.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, _cnAllTaskbarElements));

            return taskbar;
        }

        internal static void GetTaskbarAppElements()
        {
            AutomationElement taskbar = GetTaskbar();
            if (taskbar == null)
            {
                return;
            }
            
            taskbar = taskbar
                .FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, _aidTaskbarFrame));

            AutomationElementCollection taskbarSystemApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarSystemApps));
            foreach (AutomationElement app in taskbarSystemApps)
            {
                Console.WriteLine(app.Current.Name + "\n" +  
                    app.Current.BoundingRectangle.ToString());
            }

            AutomationElementCollection taskbarUserApps = taskbar
                .FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, _cnTaskbarUserApps));
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
