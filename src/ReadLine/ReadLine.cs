using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace System
{
    public static class ReadLine
    {
        public struct ReadLineResult
        {
            public string Result;
            public bool Interrupted;
        }

        private static KeyHandler _keyHandler;
        private static List<string> _history = new List<string>();
        private static bool _active;

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();
        public static Func<string, int, string[]> AutoCompletionHandler { private get; set; }
        public static bool PasswordMode { private get; set; }
        public static int InterruptInterval = 5;
        public static Func<bool> CheckInterrupt;
        public static bool IsReading => _active;

        // Wrapper for users who don't care about ReadLineResult.
        public static string Read(string prompt = "", string defaultInput = "", string initialInput = "")
        {
            ReadLineResult res = ReadExt(prompt, defaultInput, initialInput);
            return res.Result;
        }

        public static ReadLineResult ReadExt(string prompt = "", string defaultInput = "", string initialInput = "")
        {
            _active = true;
            Console.Write(prompt);

            _keyHandler = new KeyHandler(new Console2() { PasswordMode = PasswordMode }, initialInput, _history, AutoCompletionHandler);

            bool done = false;
            bool interrupted = false;

            /* We need to poll KeyAvailable very frequently so that typing doesn't feel laggy. So we cap the sleep interval at a maximum of 5 ms; if the InterruptInterval is longer than that, we set a Stopwatch to know when to call CheckInterrupt.
               (We should be calling Console.ReadKey with a timeout, but C# doesn't support that, sigh.)
            */
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
                // Handle all keys that have come in.
                while (!done && Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        done = true;
                        break;
                    }
                    _keyHandler.Handle(keyInfo);
                    Thread.Sleep(0);
                }

                // Handle the timer, if there is one.
                if (!done && CheckInterrupt != null)
                {
                    if (stopwatch == null)
                    {
                        // Check every 5ms tick.
                        interrupted = CheckInterrupt();
                    }
                    else 
                    {
                        // Check if enough time has elapsed.
                        var elapsed = stopwatch.ElapsedMilliseconds;
                        if (elapsed >= InterruptInterval)
                        {
                            stopwatch.Restart();
                            interrupted = CheckInterrupt();
                        }
                    }
                    if (interrupted)
                    {
                        done = true;
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

            _active = false;
            return new ReadLineResult() {
                Result = text,
                Interrupted = interrupted
            };
        }
    }
}
