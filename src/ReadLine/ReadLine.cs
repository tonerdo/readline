using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Collections.Generic;

namespace System
{
    public static class ReadLine
    {
        private static KeyHandler _keyHandler;
        private static List<string> _history;

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();
        public static Func<string, int, string[]> AutoCompletionHandler { private get; set; }
        public static bool PasswordMode { private get; set; }
        public static bool DisableHistory { get; set; }

        public static string Read(string prompt = "", string defaultInput = "", bool? enableHistory = null)
        {
            Console.Write(prompt);

            bool useHistory = enableHistory ?? !DisableHistory;
            var history = useHistory ? _history : new List<string>();

            _keyHandler = new KeyHandler(new Console2() { PasswordMode = PasswordMode }, history, AutoCompletionHandler);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = Console.ReadKey(true);
            }

            Console.WriteLine();

            string text = _keyHandler.Text;
            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(defaultInput))
                text = defaultInput;
            else if (useHistory)
                _history.Add(text);

            return text;
        }
    }
}
