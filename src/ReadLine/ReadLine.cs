using System;
using System.Collections.Generic;
using ReadLine.Abstractions;


namespace ReadLine
{
    public static class ReadLine
    {
        private static List<string> _history;


        static ReadLine() => _history = new List<string>();


        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }


        public static void AddHistory(params string[] text) => _history.AddRange(text);


        public static List<string> GetHistory() => _history;


        public static void ClearHistory() => _history = new List<string>();


        public static string Read(string prompt = "", string @default = "")
        {
            Console.Write(prompt);
            var keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            var text = GetText(keyHandler);

            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(@default))
                text = @default;
            else
                _history.Add(text);

            return text;
        }


        public static string ReadPassword(string prompt = "")
        {
            Console.Write(prompt);
            var keyHandler = new KeyHandler(new Console2
            {
                PasswordMode = true
            }, null, null);
            return GetText(keyHandler);
        }


        private static string GetText(KeyHandler keyHandler)
        {
            var keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            return keyHandler.Text;
        }
    }
}