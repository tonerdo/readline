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
        private static Thread _reader;
        private static ManualResetEventSlim _getInput, _gotInput;
        private static ConsoleKeyInfo  _input;
        private static object _lock = new object();
  
        static ReadLine()
        {
            _history = new List<string>();
            _getInput = new ManualResetEventSlim();
            _gotInput = new ManualResetEventSlim();
            _reader = new Thread(ReaderLoop) {
                 IsBackground = true,
                 Name = "ReadLine background reader Loop"
            };
            _reader.Start();
        }

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();
        public static bool HistoryEnabled { get; set; }
        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }
        
        public static string Read(string prompt = "", string @default = "", CancellationToken cancellationToken=default)
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = GetText(keyHandler,cancellationToken);

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

        public static string ReadPassword(string prompt = "",CancellationToken cancellationToken=default)
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, null, null);
            return GetText(keyHandler,cancellationToken);
        }

        private static string GetText(KeyHandler keyHandler,CancellationToken cancellationToken)
        {
            ConsoleKeyInfo keyInfo = ReadKeyInternal(cancellationToken);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                keyHandler.Handle(keyInfo);
                keyInfo = ReadKeyInternal(cancellationToken);
            }

            Console.WriteLine();
            return keyHandler.Text;
        }

        public static ConsoleKeyInfo ReadKey(bool intercept = false, CancellationToken cancellationToken=default)
        {
            ConsoleKeyInfo consoleKeyInfo=ReadKeyInternal(cancellationToken);
            if (!intercept)
            {
                if (consoleKeyInfo.Key==ConsoleKey.Enter)
                    Console.WriteLine();
                else
                    Console.Write(consoleKeyInfo.KeyChar);
            }
            return consoleKeyInfo;
        }

        public static Task<string> ReadAsync(string prompt,CancellationToken cancellationToken)
        {
            return Task.Run(()=>ReadLine.Read(prompt,cancellationToken: cancellationToken));
        }

        private static ConsoleKeyInfo ReadKeyInternal(CancellationToken cancellation)
        {
            if (!_gotInput.IsSet)
            {
                _getInput.Set();
                _gotInput.Wait(cancellation);
            }
            _gotInput.Reset();
            return _input;
        }
        
        private static void ReaderLoop()
        {
            while (true) {
                _getInput.Wait();
                _input = Console.ReadKey(true); 
                _getInput.Reset();
                _gotInput.Set();
            }
        }
    }
}
