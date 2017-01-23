using System;

namespace ReadLine
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;

        public static string Read()
        {
            _keyHandler = new KeyHandler();
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
