using System;
using System.Collections.Generic;
using ReadLine.Abstractions;


namespace ReadLine
{
    public class ReadLine : IReadLine
    {
        private List<string> _history;

        private ReadLine() {
            _history = new List<string>();
        }

        public static ReadLine Instance = new ReadLine(); // Singleton implementation

        public IAutoCompleteHandler AutoCompletionHandler { get; set; }


        public void AddHistory(params string[] text) => _history.AddRange(text);


        public List<string> GetHistory() => _history;


        public void ClearHistory() => _history = new List<string>();


        public string Read(string prompt = "", string @default = "")
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


        public string ReadPassword(string prompt = "")
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
            Console.TreatControlCAsInput = true;

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