using System;
using System.Collections.Generic;

namespace ReadLine
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;
        private static List<string> _history;

        public static List<string> History
        {
            get
            {
                return _history;
            }
        }

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static void AddHistory(params string[] text) => _history.AddRange(text);

        public static string Read()
        {
            _keyHandler = new KeyHandler(_history);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();
            string text = _keyHandler.Text;
            _history.Add(text);

            return text;
        }
    }
}
