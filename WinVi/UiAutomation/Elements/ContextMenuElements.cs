using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI.Misc;
using WinVi.UiAutomation;

namespace WinVi.UiAutomation.Elements
{
    /// <summary>
    /// A class responsible for finding Context menu elements from a parent UIAutomation element. 
    /// Uses parentElementHint to search for a corresponding bonding rectangle
    /// </summary>
    internal class ContextMenuElements
    {
        // How much time does it take for a new window to fully render (ControlType.Menu in this case)
        // System-dependant, user will customise it
        private static readonly int _minRenderingThreshold = 50;
        private static int _customRenderingThreshold = _minRenderingThreshold;

        private static readonly PropertyCondition _separator = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Separator);
        private static readonly PropertyCondition _menuItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem);
        private static readonly PropertyCondition _menu= new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu);

        /// <summary>
        /// Checks for context menus near the current element. 
        /// </summary>
        /// <param name="hint"></param>
        /// <exception cref="ArgumentNullException">Is thrown in case MenuItems were not found</exception>
        internal static bool GetContextMenu(string hint)
        {
            AutomationElementDictionary.Instance.GetDictionary().TryGetValue(hint, out AutomationElement rect);
            AutomationElementDictionary.Instance.Reset();
            UIKeysGenerator.Instance.Reset();

            Thread.Sleep(_customRenderingThreshold);

            // Defult Controltype.Menu element with ControlType.MenuItem items
            AutomationElement menu = AutomationElement.RootElement
                .FindFirst(TreeScope.Children, _menu);
            if (menu != null)
            {
                AutomationElementCollection menuItems = menu.FindAll(
                TreeScope.Children,
                new AndCondition(
                    new NotCondition(_separator),
                    _menuItem));
                if (menuItems != null)
                {
                    AutomationElementDictionary.Instance.AddElements(menuItems);
                    return true;
                }
            }

            // Other types of menus
            // 

            // No elements detected
            return false;
        }
        private static void testCollection(AutomationElementCollection collection)
        {
            foreach (AutomationElement el in collection)
            {
                Debug.WriteLine(el.Current.Name + ";\t" + el.Current.ClassName);
            }
        }

        private static void testElementChildren(AutomationElement element)
        {
            AutomationElementCollection collection = element.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
            Debug.WriteLine("______________");
            foreach (AutomationElement el in collection)
            {
                Debug.WriteLine(el.Current.Name + ";\t " + el.Current.ItemType + ";\t" + el.Current.ClassName);
            }
        }
    }
}
