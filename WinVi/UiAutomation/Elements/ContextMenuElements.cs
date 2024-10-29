using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVi.UiAutomation.Elements
{
    /// <summary>
    /// A class responsible for finding Context menu elements from a parent UIAutomation element. 
    /// Uses parentElementHint to search for a corresponding bonding rectangle
    /// TODO: Develop logic for finding context menu, as it is not explicitly connected to the parent AutomationElement. Might need to search in a specific radius on screen
    /// </summary>
    internal class ContextMenuElements
    {
        internal static void GetContextMenuElements(string parentElementHint)
        {
            // TODO: Context menu logic
            // The parent element (e.g. AA) should be always on display, but the child elements (Context Menu in this case, rename?)
            // should be displayed with it as well. How to detect a child element - that is a question for later
        }
    }
}
