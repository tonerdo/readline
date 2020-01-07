using System;
using System.Threading;
using System.Threading.Tasks;

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

            string input = ReadLine.Read("(prompt)> ");
            Console.WriteLine(input);

            input = ReadLine.ReadPassword("Enter Password> ");
            Console.WriteLine(input);

            var src = new CancellationTokenSource();
            src.CancelAfter(3000);
            input = ReadLine.ReadAsync("You have 3 sec to prompt: ", cancellationToken: src.Token).Result;
            if (!src.IsCancellationRequested)
                Console.WriteLine(input);
            src.Dispose();
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
}
