using System;
using System.Collections.Generic;
using System.Linq;

using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

namespace System
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;

        public static Func<string, int, string[]> AutoCompletionHandler { private get; set; }

        static ReadLine()
        {
            History = new List<string>();
        }

        public static IHistoryCollection History { get; private set; }

        public static string Read(string prompt = "")
        {
            Console.Write(prompt);

            _keyHandler = new KeyHandler(new Console2(), History, AutoCompletionHandler);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            History.Add(_keyHandler.Text);
            return _keyHandler.Text;
        }
    }
}
