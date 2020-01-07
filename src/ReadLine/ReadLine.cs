using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public static bool HistoryEnabled { get; set; }
        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }

        public static string Read(string prompt = "", string @default = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = GetText(keyHandler);

            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(@default))
            {
                text = @default;
            }
            else
            {
                if (HistoryEnabled)
                    _history.Add(text);
            }

            return text;
        }

        public static async Task<string> ReadAsync(string prompt = "", string @default = "", CancellationToken cancellationToken = default)
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = await GetTextAsync(keyHandler, cancellationToken);
            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(@default))
            {
                text = @default;
            }
            else
            {
                if (HistoryEnabled)
                    _history.Add(text);
            }
            if (cancellationToken.IsCancellationRequested)
                Console.WriteLine("");
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

        private static async Task<string> GetTextAsync(KeyHandler keyHandler, CancellationToken cancellationToken = default)
        {
            Task.Run(() =>
            {
                while (!Console.KeyAvailable)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    Thread.Sleep(50);
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                while (keyInfo.Key != ConsoleKey.Enter)
                {
                    keyHandler.Handle(keyInfo);
                    while (!Console.KeyAvailable)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return;
                        Thread.Sleep(50);
                    }
                    keyInfo = Console.ReadKey(true);
                }

                Console.WriteLine();
            }).Wait();
            return keyHandler.Text;
        }
    }
}
