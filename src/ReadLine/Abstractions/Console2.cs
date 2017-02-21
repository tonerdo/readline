using System;

namespace ReadLine.Abstractions
{
    internal class Console2 : IConsole
    {
        public int CursorLeft
        {
            get
            {
                return Console.CursorLeft;
            }
        }

        public int CursorTop
        {
            get
            {
                return Console.CursorTop;
            }
        }

        public int BufferWidth
        {
            get
            {
                return Console.BufferWidth;
            }
        }

        public int BufferHeight
        {
            get
            {
                return Console.BufferHeight;
            }
        }

        public void SetBufferSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
        }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}