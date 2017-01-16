using System;
using System.Text;

namespace ReadLine
{
    public static class ReadLine
    {
        private static int _cursorPos;
        private static int _cursorLimit;
        private static StringBuilder _text;

        static bool IsStartOfLine() => _cursorPos == 0;

        private static bool IsEndOfLine() => _cursorPos == _cursorLimit;

        private static bool IsStartOfBuffer() => Console.CursorLeft == 0;

        private static bool IsEndOfBuffer() => Console.CursorLeft == Console.BufferWidth - 1;

        private static void MoveCursorLeft()
        {
            if (IsStartOfLine())
                return;

            if (IsStartOfBuffer())
                Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
            else
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

            _cursorPos--;
        }

        private static void MoveCursorRight()
        {
            if (IsEndOfLine())
                return;

            if (IsEndOfBuffer())
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            else
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);

            _cursorPos++;
        }
    }
}
