using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Diagnostics;
using System.Threading;
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
        public static int InterruptInterval = 5;
        public static Func<bool> CheckInterrupt;

        public static string Read(string prompt = "", string defaultInput = "", string initialInput = "")
        {
            Console.Write(prompt);

            _keyHandler = new KeyHandler(new Console2() { PasswordMode = PasswordMode }, initialInput, _history, AutoCompletionHandler);

            bool done = false;

            Stopwatch stopwatch = null;
            int sleeptime = InterruptInterval;
            if (sleeptime > 5)
            {
                sleeptime = 5;
                if (CheckInterrupt != null)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                }
            }

            while (!done) {
                while (!done && Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        done = true;
                        break;
                    }
                    _keyHandler.Handle(keyInfo);
                }

                if (!done && CheckInterrupt != null)
                {
                    if (stopwatch == null)
                    {
                        // Check every tick.
                        done = CheckInterrupt();
                    }
                    else 
                    {
                        // Check if enough time has elapsed.
                        var elapsed = stopwatch.ElapsedMilliseconds;
                        if (elapsed >= InterruptInterval)
                        {
                            stopwatch.Reset();
                            done = CheckInterrupt();
                        }
                    }
                }

                if (!done)
                {
                    Thread.Sleep(sleeptime);
                }
            }

            Console.WriteLine();

            string text = _keyHandler.Text;
            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(defaultInput))
                text = defaultInput;
            else
                _history.Add(text);

            return text;
        }
    }
}
