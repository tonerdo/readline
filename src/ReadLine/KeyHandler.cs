using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.ReadLine
{
    internal class KeyHandler
    {
        private int _cursorPos;
        private int _cursorLimit;
        private int _historyIndex;
        private StringBuilder _text;
        private List<string> _history;
        private ConsoleKeyInfo _keyInfo;
        private Dictionary<string, Action> _keyActions;
        private string[] _completions;
        private int _completionStart;
        private int _completionsIdx;

        private bool IsStartOfLine() => _cursorPos == 0;

        private bool IsEndOfLine() => _cursorPos == _cursorLimit;

        private bool IsStartOfBuffer() => Console.CursorLeft == 0;

        private bool IsEndOfBuffer() => Console.CursorLeft == Console.BufferWidth - 1;
        private bool IsInAutoCompleteMode() => _completions != null;

        private void MoveCursorLeft()
        {
            if (IsStartOfLine())
                return;

            if (IsStartOfBuffer())
                Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
            else
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

            _cursorPos--;
        }

        private void MoveCursorHome()
        {
            while (!IsStartOfLine())
                MoveCursorLeft();
        }

        private string BuildKeyInput()
        {
            return _keyInfo.Modifiers != ConsoleModifiers.Control ?
                _keyInfo.Key.ToString() : _keyInfo.Modifiers.ToString() + _keyInfo.Key.ToString();
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine())
                return;

            if (IsEndOfBuffer())
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            else
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);

            _cursorPos++;
        }

        private void MoveCursorEnd()
        {
            while (!IsEndOfLine())
                MoveCursorRight();
        }

        private void ClearLine()
        {
            MoveCursorEnd();
            while (!IsStartOfLine())
                Backspace();
        }

        private void WriteNewString(string str)
        {
            ClearLine();
            foreach (char character in str)
                WriteChar(character);
        }

        private void WriteString(string str)
        {
            foreach (char character in str)
                WriteChar(character);
        }

        private void WriteChar() => WriteChar(_keyInfo.KeyChar);

        private void WriteChar(char character)
        {
            if (IsEndOfLine())
            {
                _text.Append(character);
                Console.Write(character);
                _cursorPos++;
            }
            else
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                string str = _text.ToString().Substring(_cursorPos);
                _text.Insert(_cursorPos, character);
                Console.Write(character.ToString() + str);
                Console.SetCursorPosition(left, top);
                MoveCursorRight();
            }

            _cursorLimit++;
        }

        private void Backspace()
        {
            if (!IsStartOfLine())
            {
                MoveCursorLeft();
                int index = _cursorPos;
                _text.Remove(index, 1);
                string replacement = _text.ToString().Substring(index);
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                Console.Write(string.Format("{0} ", replacement));
                Console.SetCursorPosition(left, top);
                _cursorLimit--;
            }
        }

        private void AutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            WriteString(_completions[_completionsIdx]);
            _completionsIdx++;

            if (_completionsIdx == _completions.Length)
                _completionsIdx = 0;
        }

        private void PrevHistory()
        {
            if (_historyIndex > 0)
            {
                _historyIndex--;
                WriteNewString(_history[_historyIndex]);
            }
        }

        private void NextHistory()
        {
            if (_historyIndex < _history.Count)
            {
                _historyIndex++;
                if (_historyIndex == _history.Count)
                    ClearLine();
                else
                    WriteNewString(_history[_historyIndex]);
            }
        }

        private void ResetAutoComplete()
        {
            _completions = null;
            _completionsIdx = 0;
        }

        public string Text
        {
            get
            {
                return _text.ToString();
            }
        }

        public KeyHandler(List<string> history, Func<string, int, string[]> autoCompleteHandler)
        {
            _historyIndex = history.Count;
            _history = history;
            _text = new StringBuilder();
            _keyActions = new Dictionary<string, Action>();

            _keyActions["LeftArrow"] = MoveCursorLeft;
            _keyActions["Home"] = MoveCursorHome;
            _keyActions["End"] = MoveCursorEnd;
            _keyActions["ControlA"] = MoveCursorHome;
            _keyActions["ControlB"] = MoveCursorLeft;
            _keyActions["RightArrow"] = MoveCursorRight;
            _keyActions["ControlF"] = MoveCursorRight;
            _keyActions["ControlE"] = MoveCursorEnd;
            _keyActions["Backspace"] = Backspace;
            _keyActions["ControlH"] = Backspace;
            _keyActions["ControlL"] = ClearLine;
            _keyActions["UpArrow"] = PrevHistory;
            _keyActions["ControlP"] = PrevHistory;
            _keyActions["DownArrow"] = NextHistory;
            _keyActions["ControlN"] = NextHistory;
            _keyActions["ControlU"] = () =>
            {
                while (!IsStartOfLine())
                    Backspace();
            };
            _keyActions["ControlK"] = () =>
            {
                int pos = _cursorPos;
                MoveCursorEnd();
                while (_cursorPos > pos)
                    Backspace();
            };
            _keyActions["ControlW"] = () =>
            {
                while (!IsStartOfLine() && _text[_cursorPos - 1] != ' ')
                    Backspace();
            };

            _keyActions["Tab"] = () =>
            {
                if (IsInAutoCompleteMode())
                {
                    AutoComplete();
                }
                else
                {
                    if (autoCompleteHandler == null || !IsEndOfLine())
                        return;

                    char[] anyOf = new char[] { ' ', '.', '/', '\\', ':' };
                    string text = _text.ToString();

                    _completionStart = text.LastIndexOfAny(anyOf);
                    _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                    _completions = autoCompleteHandler.Invoke(text, _completionStart);
                    if (_completions == null || _completions.Length == 0)
                        return;

                    AutoComplete();
                }
            };
        }

        public void Handle(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;

            // If in auto complete mode and Tab wasn't pressed
            if (IsInAutoCompleteMode() && _keyInfo.Key != ConsoleKey.Tab)
                ResetAutoComplete();

            Action action;
            _keyActions.TryGetValue(BuildKeyInput(), out action);
            action = action ?? WriteChar;
            action.Invoke();
        }
    }
}
