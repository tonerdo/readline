using System;
using System.Text;
using System.Collections.Generic;

namespace ReadLine
{
    internal class KeyHandler
    {
        private int _cursorPos;
        private int _cursorLimit;
        private StringBuilder _text;
        private ConsoleKeyInfo _keyInfo;
        private Dictionary<string, Action> _keyActions;

        private bool IsStartOfLine() => _cursorPos == 0;

        private bool IsEndOfLine() => _cursorPos == _cursorLimit;

        private bool IsStartOfBuffer() => Console.CursorLeft == 0;

        private bool IsEndOfBuffer() => Console.CursorLeft == Console.BufferWidth - 1;

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

        private void WriteChar()
        {
            char key = _keyInfo.KeyChar;
            if (IsEndOfLine())
            {
                _text.Append(key);
                Console.Write(key);
                _cursorPos++;
            }
            else
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                string str = _text.ToString().Substring(_cursorPos);
                _text.Insert(_cursorPos, key);
                Console.Write(key.ToString() + str);
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

        public string Text
        {
            get
            {
                return _text.ToString();
            }
        }

        public KeyHandler()
        {
            _text = new StringBuilder();
            _keyActions = new Dictionary<string, Action>();

            _keyActions["LeftArrow"] = MoveCursorLeft;
            _keyActions["Home"] = MoveCursorHome;
            _keyActions["Home"] = MoveCursorEnd;
            _keyActions["ControlA"] = MoveCursorHome;
            _keyActions["ControlB"] = MoveCursorLeft;
            _keyActions["RightArrow"] = MoveCursorRight;
            _keyActions["ControlF"] = MoveCursorRight;
            _keyActions["ControlE"] = MoveCursorEnd;
            _keyActions["Backspace"] = Backspace;
        }

        public void Handle(ConsoleKeyInfo keyInfo)
        {
            _keyInfo = keyInfo;
            Action action;
            _keyActions.TryGetValue(BuildKeyInput(), out action);
            action = action ?? WriteChar;
            action.Invoke();
        }
    }
}