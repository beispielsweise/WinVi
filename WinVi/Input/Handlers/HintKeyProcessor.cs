using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinVi.Input.Handlers.Modes;
using WinVi.Input.Managers.Utilities;
using WinVi.UI;
using WinVi.UiAutomation;
using static WinVi.Input.Handlers.Modes.TaskbarMode;

namespace WinVi.Input.Handlers
{
    internal class HintKeyProcessor
    {
        private static string _currentSequence = string.Empty;

        // Return types, used to set TrayIcon status
        public enum HintKeyStatus
        {
            LeftClickPressed,
            RightClickPressed,
            Error,
            Skip
        }

        /// <summary>
        /// Processes a hint key, that user pressed
        /// </summary>
        /// <param name="vkString">Key name</param>
        /// <param name="_shiftPressed">Is used to determine the right click, e.g. user pressing AA or aA or A</param>
        /// <returns>A status tat is passed to TrayIcon</returns>
        internal static HintKeyStatus ProcessHintKey(string vkString, bool _shiftPressed, out string hint)
        {
            if (vkString.Equals(KeyboardHookUtilities.shiftKeyName))
            {
                hint = string.Empty;
                return HintKeyStatus.Skip;
            }

            _currentSequence += vkString;
            hint = _currentSequence;
            if (AutomationElementDictionary.Instance.ContainsKey(_currentSequence))
            {
                if (AutomationElementDictionary.Instance.TryGetValue(_currentSequence, out Rect rect))
                {
                    OverlayWindow.Instance.Hide();
                    if (ContextMenuSubmode.GetContextMenuStatus())
                        _shiftPressed = false;
                    ClickManager.Instance.Click(rect, _shiftPressed, false);
                }

                _currentSequence = string.Empty;

                if (!_shiftPressed)
                    return HintKeyStatus.LeftClickPressed;
                return HintKeyStatus.RightClickPressed;
            }
            else if (_currentSequence.Length >= 2)
            {
                _currentSequence = string.Empty;
                return HintKeyStatus.Error;
            }
            else
            {
                return HintKeyStatus.Skip;
            }
        }

        /// <summary>
        /// Resets current hint counter
        /// </summary>
        internal static void ResetCurrentSequence()
        {
            _currentSequence = null;
        }
    }
}
