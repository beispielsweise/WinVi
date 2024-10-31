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
        private static readonly int _minRenderingThreshold = 100;
        private static int _customRenderingThreshold = _minRenderingThreshold;

        /// <summary>
        /// Checks for context menus near the current element. 
        /// </summary>
        /// <param name="hint"></param>
        /// <exception cref="ArgumentNullException">Is thrown in case MenuItems were not found</exception>
        internal static void GetContextMenu(string hint)
        {
            AutomationElementDictionary.Instance.GetDictionary().TryGetValue(hint, out Rect rect);
            AutomationElementDictionary.Instance.Reset();
            UIKeysGenerator.Instance.Reset();

            Thread.Sleep(_customRenderingThreshold);

            // Defult Controltype.Menu element with ControlType.MenuItem items
            AutomationElement menu = AutomationElement.RootElement
                .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu))
                ?? throw new Exception("Increase rendering Threshold, no menu");
            AutomationElementCollection menuItems = menu.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsInvokePatternAvailableProperty, true))
                ?? throw new Exception("Increase rendering Threshold, no menu elements");

            if (menuItems != null)
                AutomationElementDictionary.Instance.AddElements(menuItems);

            // Other types of menus

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
