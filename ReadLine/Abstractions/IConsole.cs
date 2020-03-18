namespace ReadLine.Abstractions
{
    public interface IConsole
    {
        bool PasswordMode { get; set; }
        int CursorLeft { get; }
        int CursorTop { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        void SetCursorPosition(int left, int top);
        void SetBufferSize(int width, int height);
        void Write(string value);
        void WriteLine(string value);
    }
}