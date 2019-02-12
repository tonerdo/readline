using System;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ReadLine Library Demo");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string[] history = new string[] { "ls -a", "dotnet run", "git init" };
            ReadLine.AddHistory(history);

            ReadLine.AutoCompletionHandler = new AutoCompletionHandler();
            ReadLine.HistoryEnabled = true;

            string input;

            #pragma warning disable 162
            if (false)
            {
                DefaultDemo();
            }
            else
            {
                TimeoutDemo();
            }
            #pragma warning restore 162
            
            input = ReadLine.ReadPassword("Enter Password> ");
            Console.WriteLine(input);


            void DefaultDemo()
            {
                do
                {
                    input = ReadLine.Read("(prompt)> ");
                    Console.WriteLine(input);
                } while (!input.Equals("quit"));
            }


            void TimeoutDemo()
            {
                ReadLineWithTimeout readLine = new ReadLineWithTimeout();
                TimeSpan timeout = TimeSpan.FromSeconds(3);
                do
                {
                    bool success;
                    (success, input) = readLine.TryRead("(prompt)> ", timeout.Ticks / TimeSpan.TicksPerMillisecond);
                    if (!success)
                    {
                        input = "(timed out)";
                    }
                    Console.WriteLine(input);
                } while (!input.Equals("quit"));
            }
        }
    }

    class AutoCompletionHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/', '\\', ':' };
        public string[] GetSuggestions(string text, int index)
        {
            if (text.StartsWith("git "))
                return new string[] { "init", "clone", "pull", "push" };
            else
                return null;
        }
    }
    
    
    public class ReadLineWithTimeout {
  
        private AutoResetEvent producerGreenLight;
        private AutoResetEvent consumerGreenLight;
        private string input;
        private string prompt = "";


        public ReadLineWithTimeout() {
            producerGreenLight = new AutoResetEvent(false);
            consumerGreenLight = new AutoResetEvent(false);
            Thread t = new Thread(() => { reader(); });
            t.Name = "ConsoleReader";
            t.IsBackground = true;
            t.Start();
        }

  
        private void reader() {
            ReadLine.HistoryEnabled = true;
            while (true) {
                producerGreenLight.WaitOne();
                input = ReadLine.Read(prompt);
                consumerGreenLight.Set();
            }
        }


        public string Read(string prompt = "", long timeOutMillisecs = Timeout.Infinite) {
            var result = TryRead(prompt, timeOutMillisecs);
            if (!result.success) throw new TimeoutException();
            return result.line;
        }
  
  
        public (bool success, string line) TryRead(string prompt = "", long timeOutMillisecs = Timeout.Infinite) {
            this.prompt = prompt;
            producerGreenLight.Set();
            if (!consumerGreenLight.WaitOne(TimeSpan.FromMilliseconds(timeOutMillisecs)))
                return (false, null);
            return (true, input);
        }

    }

}
