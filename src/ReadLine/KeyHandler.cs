using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using ReadLine.Abstractions;

namespace ReadLine
{
    internal class KeyHandler
    {
        private int _cursorPos;
        private int _cursorLimit;
        private readonly StringBuilder _text;
        private readonly List<string> _history;
        private int _historyIndex;
        private ConsoleKeyInfo _keyInfo;
        private readonly Dictionary<string, Action> _keyActions;
        private string[] _completions;
        private int _completionStart;
        private int _completionsIndex;
        private readonly IConsole Console2;

        private bool IsStartOfLine()
        {
            return _cursorPos == 0;
        }

        private bool IsEndOfLine()
        {
            return _cursorPos == _cursorLimit;
        }

        private bool IsStartOfBuffer()
        {
            return Console2.CursorLeft == 0;
        }

        private bool IsEndOfBuffer()
        {
            return Console2.CursorLeft == Console2.BufferWidth - 1;
        }
        private bool IsInAutoCompleteMode()
        {
            return _completions != null;
        }

        private void MoveCursorLeft()
        {
            if (IsStartOfLine())
                return;

            if (IsStartOfBuffer())
                Console2.SetCursorPosition(Console2.BufferWidth - 1, Console2.CursorTop - 1);
            else
                Console2.SetCursorPosition(Console2.CursorLeft - 1, Console2.CursorTop);

            _cursorPos--;
        }

        private void MoveCursorHome()
        {
            while (!IsStartOfLine())
                MoveCursorLeft();
        }

        private string BuildKeyInput()
        {
            return (_keyInfo.Modifiers != ConsoleModifiers.Control && _keyInfo.Modifiers != ConsoleModifiers.Shift) ?
                _keyInfo.Key.ToString() : _keyInfo.Modifiers + _keyInfo.Key.ToString();
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine())
                return;

            if (IsEndOfBuffer())
                Console2.SetCursorPosition(0, Console2.CursorTop + 1);
            else
                Console2.SetCursorPosition(Console2.CursorLeft + 1, Console2.CursorTop);

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
            foreach (var character in str)
                WriteChar(character);
        }

        private void WriteString(string str)
        {
            foreach (var character in str)
                WriteChar(character);
        }

        private void WriteChar()
        {
            WriteChar(_keyInfo.KeyChar);
        }

        private void WriteChar(char character)
        {
            if (IsEndOfLine())
            {
                _text.Append(character);
                Console2.Write(character.ToString(CultureInfo.InvariantCulture));
                _cursorPos++;
            }
            else
            {
                var left = Console2.CursorLeft;
                var top = Console2.CursorTop;
                var str = _text.ToString().Substring(_cursorPos);
                _text.Insert(_cursorPos, character);
                Console2.Write(character + str);
                Console2.SetCursorPosition(left, top);
                MoveCursorRight();
            }

            _cursorLimit++;
        }

        private void Backspace()
        {
            if (IsStartOfLine())
                return;

            MoveCursorLeft();
            var index = _cursorPos;
            _text.Remove(index, 1);
            var replacement = _text.ToString().Substring(index);
            var left = Console2.CursorLeft;
            var top = Console2.CursorTop;
            Console2.Write(string.Format("{0} ", replacement));
            Console2.SetCursorPosition(left, top);
            _cursorLimit--;
        }

        private void StartAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        private void NextAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex++;

            if (_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            WriteString(_completions[_completionsIndex]);
        }

        private void PreviousAutoComplete()
        {
            while (_cursorPos > _completionStart)
                Backspace();

            _completionsIndex--;

            if (_completionsIndex == -1)
                _completionsIndex = _completions.Length - 1;

            WriteString(_completions[_completionsIndex]);
        }

        private void PrevHistory()
        {
            if (_historyIndex <= 0)
                return;

            _historyIndex--;
            WriteNewString(_history[_historyIndex]);
        }

        private void NextHistory()
        {
            if (_historyIndex >= _history.Count)
                return;

            _historyIndex++;
            if (_historyIndex == _history.Count)
                ClearLine();
            else
                WriteNewString(_history[_historyIndex]);
        }

        private void ResetAutoComplete()
        {
            _completions = null;
            _completionsIndex = 0;
        }

        public string Text
        {
            get
            {
                return _text.ToString();
            }
        }

        public KeyHandler(IConsole console, List<string> history, Func<string, int, string[]> autoCompleteHandler)
        {
            Console2 = console;

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
                var pos = _cursorPos;
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
                    NextAutoComplete();
                }
                else
                {
                    if (autoCompleteHandler == null || !IsEndOfLine())
                        return;

                    var anyOf = new[] { ' ', '.', '/', '\\', ':' };
                    var text = _text.ToString();

                    _completionStart = text.LastIndexOfAny(anyOf);
                    _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                    _completions = autoCompleteHandler.Invoke(text, _completionStart);
                    if (_completions == null || _completions.Length == 0)
                        return;

                    StartAutoComplete();
                }
            };

            _keyActions["ShiftTab"] = () =>
            {
                if (IsInAutoCompleteMode())
                {
                    PreviousAutoComplete();
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
