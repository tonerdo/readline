using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Collections.Generic;

namespace System
{
    public static class ReadLine
    {
        private static List<string> _history;

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();
        public static bool PasswordMode { private get; set; }
        public static bool DisableHistory { get; set; }
        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }

        public static string Read(string prompt = "", string defaultInput = "", bool? enableHistory = null)
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = GetText(keyHandler);

            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(defaultInput))
                text = defaultInput;
            else
                _history.Add(text);

            return text;
        }

        public static string ReadPassword(string prompt = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, null, null);
            return GetText(keyHandler);
        }

        private static string GetText(KeyHandler keyHandler)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
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
