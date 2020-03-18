using ReadLine.Abstractions;


namespace ReadLine.Tests.Abstractions
{
    internal class Console2 : IConsole
    {
        public bool PasswordMode { get; set; }

        public int CursorLeft { get; private set; }

        public int CursorTop { get; private set; }

        public int BufferWidth { get; private set; }

        public int BufferHeight { get; private set; }


        public Console2()
        {
            CursorLeft = 0;
            CursorTop = 0;
            BufferWidth = 100;
            BufferHeight = 100;
        }

        public void SetBufferSize(int width, int height)
        {
            BufferWidth = width;
            BufferHeight = height;
        }

        public void SetCursorPosition(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }

        public void Write(string value)
        {
            CursorLeft += value.Length;
        }

        public void WriteLine(string value)
        {
            CursorLeft += value.Length;
        }
    }
}