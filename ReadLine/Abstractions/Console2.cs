using System;


namespace ReadLine.Abstractions
{
    internal class Console2 : IConsole
    {
        public bool PasswordMode { get; set; }


        public int CursorLeft => Console.CursorLeft;


        public int CursorTop => Console.CursorTop;


        public int BufferWidth => Console.BufferWidth;


        public int BufferHeight => Console.BufferHeight;


        public void SetBufferSize(int width, int height) => Console.SetBufferSize(width, height);


        public void SetCursorPosition(int left, int top)
        {
            if (!PasswordMode)
                Console.SetCursorPosition(left, top);
        }


        public void Write(string value)
        {
            if (PasswordMode)
                value = new string(default(char), value.Length);

            Console.Write(value);
        }


        public void WriteLine(string value) => Console.WriteLine(value);
    }
}