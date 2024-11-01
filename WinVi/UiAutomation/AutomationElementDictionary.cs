﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using WinVi.UI.Misc;

namespace WinVi.UiAutomation
{
    /// <summary>
    /// Holds AutomationElementsDictionary with all elements to be displayed on the overlay. 
    /// Is accessed by multiple classes, but only one at a time
    /// </summary>
    internal class AutomationElementDictionary : IDisposable
    {
        private static readonly Lazy<AutomationElementDictionary> _instance = new Lazy<AutomationElementDictionary>(() => new AutomationElementDictionary(), true);
        internal static AutomationElementDictionary Instance => _instance.Value;

        private Dictionary<string, Rect> _automationElementDict = new Dictionary<string, Rect>();
       
        /// <summary>
        /// Retrievs an instance of the dictionary.
        /// </summary>
        /// <returns>Dictionary<string, Rect></returns>
        internal IReadOnlyDictionary<string, Rect> GetDictionary() { return _automationElementDict; }

        /// <summary>
        /// Adds data from a AutomationElementCollection to the _automationELementDict. Last element can be excluded
        /// UIKeysGenerator is to be reset manually in the parent function.
        /// </summary>
        /// <param name="collection">AutomationElementCollection: collection to be added</param>
        /// <param name="removeLastElement">bool: does last element need to be excluded</param>
        internal void AddElements(AutomationElementCollection collection, bool removeLastElement = false)
        {
            for (int i = 0; i < (removeLastElement ? collection.Count - 1 : collection.Count); i++)
            {
                _automationElementDict.Add(
                    UIKeysGenerator.Instance.GetNextKey(),
                    collection[i].Current.BoundingRectangle);
            }
        }

        /// <summary>
        /// Clears AutomationELementsDictionary.
        /// </summary>
        internal void Reset()
        {
            _automationElementDict = new Dictionary<string, Rect>(); 
        }

        internal bool ContainsKey(string key)
        {
            return _automationElementDict.ContainsKey(key);
        }

        internal bool TryGetValue(string key, out Rect value)
        {
            return _automationElementDict.TryGetValue(key, out value);
        }

        // ??? Possibly not needed, pre-save elements (e.g. for context menus)
        internal void SaveElement()
        {

        }

        public override string ToString()
        {
            string result = "";
            foreach (string hint in _automationElementDict.Keys)
                result += ( hint + "; " + _automationElementDict.TryGetValue(hint, out Rect rect)) + "\n";

            return result;
        }

        public void Dispose()
        {
            _automationElementDict = null;
        }
    }
}
