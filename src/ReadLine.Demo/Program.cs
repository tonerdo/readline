using System;

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
            ReadLine.History.Add(history);

            ReadLine.AutoCompletionHandler = (t, s) =>
            {
                if (t.StartsWith("git "))
                    return new string[] { "init", "clone", "pull", "push" };
                else
                    return null;
            };

            // VS.NET attached debbugger needes this event to break the loop.
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs eventArgs)
            {
              Console.WriteLine("[Ctrl] + [C] detected. Closing app ...");
              Environment.Exit(0);
            };

            Console.WriteLine("Press [Ctrl] + [C] to break prompt loop.");
            while (true)
            {
              string input = ReadLine.Read("(prompt)> ");
              Console.Write(input);
            }
        }
    }
}
