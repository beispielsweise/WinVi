using System;

namespace WinVi.UI.Misc
{
    /// <summary>
    /// A class that generates key for a dictionary of UI Automation elements to be displayed on the screen.
    /// It is intended to be used ONLY with usual letters, like qwerty for the sake of simplicity. 
    /// An option to exclude keys is not available, but it can be modified
    /// </summary>
    internal class UIKeysGenerator
    {
        private static readonly Lazy<UIKeysGenerator> _instance = new Lazy<UIKeysGenerator>(() => new UIKeysGenerator(), true);

        private static readonly string _commonCharactersString = "ASDFGHJKL"; // Commonly used characters in the middle line
        private static readonly string _extendedCharactersString = "ZXCVBNMQWERTYUIOP"; // In some rare cases, to make sure there are enough characters. Will be deleted though. 

        // Final array of available keys
        private readonly string[] _keys;
        // Index to determine the current iteration through _keys
        private int _index;

        internal static UIKeysGenerator Instance => _instance.Value;

        private UIKeysGenerator()
        {
            char[] charCommonKeys = _commonCharactersString.ToCharArray();
            char[] charExtendedKeys = _extendedCharactersString.ToCharArray();

            _keys = new string[charCommonKeys.Length * charCommonKeys.Length + charExtendedKeys.Length * charExtendedKeys.Length];

            _index = 0;
            foreach (char firstC in charCommonKeys)
            {
                foreach (char secondC in charCommonKeys)
                    _keys[_index++] = $"{firstC}{secondC}";
            }

            foreach (char firstC in charExtendedKeys)
            {
                foreach (char secondC in charExtendedKeys)
                    _keys[_index++] = $"{firstC}{secondC}";
            }
            _index = 0;
        }

        /// <summary>
        /// Gets keys to be used with corresponging UI Elements
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal string GetNextKey()
        {
            if (_index >= _keys.Length)
                throw new InvalidOperationException("No more keys to be generated");

            return _keys[_index++];
        }

        /// <summary>
        /// Forse-resets _index value
        /// </summary>
        internal void Reset()
        {
            _index = 0;
        }
    }
}
