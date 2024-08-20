using System;
using System.Windows.Forms;

namespace WinVi.UiAutomation.Utilities
{
    /// <summary>
    /// A class that generates key for a dictionary of UI Automation elements to be displayed on the screen.
    /// It is intended to be used ONLY with usual letters, like qwerty for the sake of simplicity. 
    /// An option to exclude keys is not available, but it can be modified
    /// </summary>
    internal class UIKeysGenerator
    {
        private static readonly Lazy<UIKeysGenerator> _instance = new Lazy<UIKeysGenerator>(() => new UIKeysGenerator(), true);
        private readonly string _commonCharactersString = "ASDFGHJKL";
        private readonly string _extendedCharactersString = "ZXCVBNMQWERTYUIOP";

        private string[] _keys;
        private int _index;
      
        internal static UIKeysGenerator Instance => _instance.Value;
        
        private UIKeysGenerator()
        {
            char[] charCommonKeys = _commonCharactersString.ToCharArray();
            char[] charExtendedKeys = _extendedCharactersString.ToCharArray();

            // Number of single-letter keys + two-letter combinations
            int singleCommonKeys = charCommonKeys.Length;  // A, S, D, F, etc.
            int singleExtendedKeys = charExtendedKeys.Length;  // Z, X, C, V, etc.

            int twoCommonKeys = charCommonKeys.Length * charCommonKeys.Length;  // AA, AS, AD, etc.
            int twoExtendedKeys = charExtendedKeys.Length * charExtendedKeys.Length;  // ZZ, ZX, ZC, etc.

            _keys = new string[singleCommonKeys + singleExtendedKeys + twoCommonKeys + twoExtendedKeys];

            // Adding common keys
            int index = 0;
            foreach (char firstC in charCommonKeys)
            {
                _keys[index++] = $"{firstC}";
            }

            foreach (char firstC in charCommonKeys)
            {
                foreach (char secondC in charCommonKeys)
                    _keys[index++] = $"{firstC}{secondC}";
            }

            // Adding additional keys, for when 256 isnt enough
            foreach (char firstC in charExtendedKeys)
            {
                _keys[index++] = $"{firstC}";
            }

            foreach (char firstC in charExtendedKeys)
            {
                foreach (char secondC in charExtendedKeys)
                    _keys[index++] = $"{firstC}{secondC}";
            }
            _index = 0;
        }

        internal string GetNextKey()
        {
            if (_index >= _keys.Length)
                throw new InvalidOperationException("No more keys to be generated");

            return _keys[_index++];
        }

        internal void Reset()
        {
            _index = 0;
        }
    }
}
