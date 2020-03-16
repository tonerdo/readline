using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Collections.Generic;

namespace System
{
    public static class ReadLine
    {
        public const string CTRL_C = "^C";

        private static List<string> _history;

        private static bool _cancelEnabled = false;

        private static bool _cancelPressed = false;

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static bool CancelKeyPressEnabled
        {
            get { return _cancelEnabled; }
            set
            {
                if (_cancelEnabled = value)
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleConsoleCancelEvent);
                else
                    Console.CancelKeyPress -= new ConsoleCancelEventHandler(HandleConsoleCancelEvent);
            }
        }

        private static void HandleConsoleCancelEvent(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _cancelPressed = true;
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

        public static string ReadPassword(string prompt = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, null, null);
            return GetText(keyHandler);
        }

        private static string GetText(KeyHandler keyHandler)
        {
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            do
            {
                while (true)
                {
                    if (Console.KeyAvailable || _cancelPressed)
                        break;
                    System.Threading.Thread.Sleep(200);
                }

                if (!_cancelPressed)
                {
                    keyInfo = Console.ReadKey(true);
                    keyHandler.Handle(keyInfo);
                }

            } while (!_cancelPressed && keyInfo.Key != ConsoleKey.Enter);


            if (_cancelPressed)
            {
                keyHandler.ClearText();
                Console.WriteLine(CTRL_C);
                _cancelPressed = false;
                return CTRL_C;
            }
            else
            {
                Console.WriteLine();
                return keyHandler.Text;
            }
        }
    }
}
