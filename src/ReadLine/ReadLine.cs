using System;

namespace ReadLine
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;

        static ReadLine()
        {
            _keyHandler = new KeyHandler();
        }

        public static string Read()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            return _keyHandler.Text;
        }
    }
}
