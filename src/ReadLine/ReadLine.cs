using System;
using System.Collections.Generic;

namespace ReadLine
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;

        public static List<string> History { get; }

        static ReadLine()
        {
            History = new List<string>();
        }

        public static string Read()
        {
            _keyHandler = new KeyHandler(History);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            string text = _keyHandler.Text;
            History.Add(text);

            return text;
        }
    }
}
