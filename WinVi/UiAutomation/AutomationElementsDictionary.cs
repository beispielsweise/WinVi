using System;
using System.Collections.Generic;
using System.Windows;

namespace WinVi.UiAutomation
{
    /// <summary>
    /// Holds AutomationElementsDictionary with all elements to be displayed on the overlay. 
    /// Is accessed by multiple classes, but only one at a time
    /// </summary>
    internal class AutomationElementsDictionary : IDisposable
    {
        private static readonly Lazy<AutomationElementsDictionary> _instance = new Lazy<AutomationElementsDictionary>(() => new AutomationElementsDictionary(), true);
        internal static AutomationElementsDictionary Instance => _instance.Value;

        private Dictionary<string, Rect> _automationElementsDict = new Dictionary<string, Rect>();
       
        /// <summary>
        /// Retrievs an instance of the dictionary
        /// </summary>
        /// <returns>Dictionary<string, Rect></returns>
        internal IReadOnlyDictionary<string, Rect> GetDictionary() { return _automationElementsDict; }

        /// <summary>
        /// Adds an entry to AutmationElementsDictionary
        /// </summary>
        /// <param name="key">string: Hint key</param>
        /// <param name="value">Rect: Bonding rectangle</param>
        internal void AddElement(string key, Rect value)
        {
            _automationElementsDict.Add(key, value);
        }

        /// <summary>
        /// Clears AutomationELementsDictionary
        /// </summary>
        internal void Clear()
        {
            _automationElementsDict ??= new Dictionary<string, Rect>(); 
        }

        internal bool ContainsKey(string key)
        {
            return _automationElementsDict.ContainsKey(key);
        }

        internal bool TryGetValue(string key, out Rect value)
        {
            return _automationElementsDict.TryGetValue(key, out value);
        }

        // ??? Possibly not needed, pre-save elements (e.g. for context menus)
        internal void SaveElement()
        {

        }

        public void Dispose()
        {
            _automationElementsDict = null;
        }
    }
}
