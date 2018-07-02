using Internal.ReadLine.Abstractions;

using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.ReadLine
{
    internal class KeyHandler
    {
        private int _promptLength;
        private StringBuilder _text;
        private List<string> _history;
        private int _historyIndex;
        private ConsoleKeyInfo _keyInfo;
        private Dictionary<string, Action> _keyActions;
        private string[] _completions;
        private int _completionStart;
        private int _completionsIndex;
        private IConsole Console2;

        private bool IsStartOfLine() => Console2.CursorLeft == 0;

        private bool IsEndOfLine() => Console2.CursorLeft == _text.Length + _promptLength;

        private bool IsStartOfBuffer() => Console2.CursorLeft == 0;

        private bool IsEndOfBuffer() => Console2.CursorLeft == Console2.BufferWidth - 1;
        private bool IsInAutoCompleteMode() => _completions != null;

        private void MoveCursorLeft()
        {
            if (IsStartOfLine())
                return;

            if (Console2.CursorLeft == _promptLength)
                return;

            if (IsStartOfBuffer())
                Console2.SetCursorPosition(Console2.BufferWidth - 1, Console2.CursorTop - 1);
            else
                Console2.SetCursorPosition(Console2.CursorLeft - 1, Console2.CursorTop);
        }

        private void MoveCursorHome()
        {
            Console2.SetCursorPosition(_promptLength, Console2.CursorTop);
        }

        private string BuildKeyInput()
        {
            return (_keyInfo.Modifiers != ConsoleModifiers.Control && _keyInfo.Modifiers != ConsoleModifiers.Shift) ?
                _keyInfo.Key.ToString() : _keyInfo.Modifiers.ToString() + _keyInfo.Key.ToString();
        }

        private void MoveCursorRight()
        {
            if (IsEndOfLine())
            {
                return;
            }

            if (IsEndOfBuffer())
            {
                Console2.SetCursorPosition(0, Console2.CursorTop + 1);
            }
            else
            {
                Console2.SetCursorPosition(Console2.CursorLeft + 1, Console2.CursorTop);
            }
        }

        private void MoveCursorEnd()
        {
            while (!IsEndOfLine())
                MoveCursorRight();
        }

        private void ClearLine()
        {
            ClearLine(_promptLength);
        }
                
        private void ClearLine(int startPos)
        {
            var clear = "";
            for (var i = 0; i <  Console2.BufferWidth - startPos; i++)
            {
                clear += " ";
            }
            Console2.SetCursorPosition(startPos, Console2.CursorTop);
            Console2.Write(clear);
            Console2.SetCursorPosition(startPos, Console2.CursorTop);
        }

        private void WriteString(string str)
        {
            _text.Clear();
            _text.Append(str);
            Console2.Write(_text.ToString());
        }

        private void WriteChar() => WriteChar(_keyInfo.KeyChar);

        private void WriteChar(char c)
        {
            if (Console2.CursorLeft >= _text.Length + _promptLength)
            {
                _text.Append(c);
                Console2.Write(c.ToString());
            }
            else
            {
                int origPos = Console2.CursorLeft;
                _text.Insert(origPos - _promptLength, c);
                ClearLine();
                Console2.Write(_text.ToString());
                Console2.SetCursorPosition(origPos + 1, Console2.CursorTop);
            }
        }

        private void Backspace()
        {
            if (IsStartOfLine())
            {
                ResetAutoComplete();
                return;
            }
            if (Console2.CursorLeft > _promptLength)
            {
                int origPos = Console2.CursorLeft;
                _text.Remove(origPos - _promptLength - 1, 1);
                ClearLine();
                WriteString(_text.ToString());
                Console2.SetCursorPosition(origPos -1, Console2.CursorTop);
            }
        }

        private void Delete()
        {
            if (IsEndOfLine())
            {
                return;
            }

            int origPos = Console2.CursorLeft;
            if ((Console2.CursorLeft - _promptLength) < _text.Length)
            {
                _text.Remove(Console2.CursorLeft - _promptLength, 1);
                ClearLine();
                WriteString(_text.ToString());
                Console2.SetCursorPosition(origPos, Console2.CursorTop);
            }
        }

        private void TransposeChars()
        {
            bool almostEndOfLine() => (Console2.BufferWidth - Console2.CursorLeft) == 1;
            int incrementIf(Func<bool> expression, int index) =>  expression() ? index + 1 : index;
            int decrementIf(Func<bool> expression, int index) => expression() ? index - 1 : index;

            if (IsStartOfLine()) { return; }

            var firstIdx = decrementIf(IsEndOfLine, Console2.CursorLeft - 1);
            var secondIdx = decrementIf(IsEndOfLine, Console2.CursorLeft);

            var secondChar = _text[secondIdx];
            _text[secondIdx] = _text[firstIdx];
            _text[firstIdx] = secondChar;

            var left = incrementIf(almostEndOfLine, Console2.CursorLeft);
            var cursorPosition = incrementIf(almostEndOfLine, Console2.CursorLeft);

            ClearLine();
            WriteString(_text.ToString());

            Console2.SetCursorPosition(left, Console2.CursorTop);
            MoveCursorRight();
        }

        private void StartAutoComplete()
        {
            ClearLine(_completionStart + _promptLength);
            _completionsIndex = 0;
            
            WriteAutoComplete();
        }

        private void NextAutoComplete()
        {
            ClearLine(_completionStart + _promptLength);
            _completionsIndex++;

            if (_completionsIndex == _completions.Length)
                _completionsIndex = 0;

            WriteAutoComplete();
        }

        private void PreviousAutoComplete()
        {
            ClearLine(_completionStart + _promptLength);
            _completionsIndex--;

            if (_completionsIndex == -1)
                _completionsIndex = _completions.Length - 1;

            WriteAutoComplete();
        }

        private void WriteAutoComplete()
        {
            if (_text.ToString().Contains(" "))
            {
                var separator = _text.ToString().LastIndexOf(' ');
                ClearLine();
                WriteString(_text.ToString().Substring(0, separator) + " " + _completions[_completionsIndex]);
            }
            else
            {
                WriteString(_completions[_completionsIndex]);
            }
        }

        private void PrevHistory()
        {
            if (_historyIndex > 0)
            {
                _historyIndex--;
                ClearLine();
                WriteString(_history[_historyIndex]);
            }
        }

        private void NextHistory()
        {
            if (_historyIndex < _history.Count)
            {
                _historyIndex++;
                ClearLine();
                if (_historyIndex != _history.Count)
                {
                    WriteString(_history[_historyIndex]);
                }
            }
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

        public KeyHandler(IConsole console, List<string> history, IAutoCompleteHandler autoCompleteHandler, int promptLength)
        {
            _promptLength = promptLength;
            Console2 = console;

            _history = history ?? new List<string>();
            _historyIndex = _history.Count;
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
            _keyActions["Delete"] = Delete;
            _keyActions["ControlD"] = Delete;
            _keyActions["ControlH"] = Backspace;
            _keyActions["ControlL"] = ClearLine;
            _keyActions["Escape"] = ClearLine;
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
                ClearLine(Console2.CursorLeft);
            };
            _keyActions["ControlW"] = () =>
            {
                while (!IsStartOfLine() && _text[Console2.CursorLeft - 1] != ' ')
                    Backspace();
            };
            _keyActions["ControlT"] = TransposeChars;

            _keyActions["Tab"] = () =>
            {
                if (IsInAutoCompleteMode())
                {
                    NextAutoComplete();
                }
                else
                {
                    if (autoCompleteHandler == null)
                        return;

                    string text = _text.ToString();

                    _completionStart = text.LastIndexOfAny(autoCompleteHandler.Separators);
                    _completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

                    _completions = autoCompleteHandler.GetSuggestions(text, _completionStart);
                    _completions = _completions?.Length == 0 ? null : _completions;

                    if (_completions == null)
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
            {
                ResetAutoComplete();
            }

            Action action;
            _keyActions.TryGetValue(BuildKeyInput(), out action);
            action = action ?? WriteChar;
            action.Invoke();
        }
    }
}