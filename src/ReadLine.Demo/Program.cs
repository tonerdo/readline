using System;
using System.Text;
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

            using (CancellationTokenSource cts = new CancellationTokenSource(5000))
            {
                
                try {
                    input = ReadLine.Read($"prompt [5 sec]> ",cancellationToken: cts.Token);
                    Console.WriteLine($"Result: {input}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine(" [Operation was aborted]");
                }
            }
            
            int interrupted=0;
            Console.Write("Reading several lines using ReadKey [Terminate with CTRL+Q]: ");
            StringBuilder sb = new StringBuilder();
            while (true)
                using (CancellationTokenSource cts=new CancellationTokenSource(10)) //10 ms to stress the loop
                {
                    try
                    {
                        var rki=ReadLine.ReadKey(cancellationToken: cts.Token);
                        if (rki.KeyChar=='\u0011') // CTRL-Q in unicode
                            break;
                        sb.Append(rki.KeyChar);
                        if (rki.KeyChar=='\r')
                            sb.Append('\n');
                    }
                    catch (OperationCanceledException)
                    {
                        interrupted++;
                    }
                }
            Console.WriteLine("ReadKey was interrupted {0} times during read",interrupted);
            Console.WriteLine("Entered value:");
            Console.WriteLine("---------------------");
            Console.WriteLine(sb.ToString());
            Console.WriteLine("---------------------");
            
            using (CancellationTokenSource cts = new CancellationTokenSource(5000))
            {
                try {
                    input=ReadLine.ReadAsync("Reading line in a Task (5 secs before cancellation)>",cts.Token).GetAwaiter().GetResult();
                    Console.WriteLine(input);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine(" [Interupted by timer]");
                }
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
}
