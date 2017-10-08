using System;
using System.Collections.Generic;
using ReadLine.Abstractions;

namespace ReadLine
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;
        private static List<string> _history;

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static Func<string, int, string[]> AutoCompletionHandler { private get; set; }
        public static bool PasswordMode { private get; set; }

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();

        public static string Read(string prompt = "", string defaultInput = "")
        {
            Console.Write(prompt);

            _keyHandler = new KeyHandler(new Console2
            {
                PasswordMode = PasswordMode
            }, _history, AutoCompletionHandler);
            var keyInfo = Console.ReadKey(true);

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            var text = _keyHandler.Text;
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(defaultInput))
                text = defaultInput;
            else
                _history.Add(text);

            return text;
        }
    }
}