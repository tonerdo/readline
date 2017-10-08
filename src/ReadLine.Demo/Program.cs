using System;

namespace ReadLine.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ReadLine Library Demo");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string[] history =
            {
                "ls -a",
                "dotnet run",
                "git init"
            };
            ReadLine.AddHistory(history);

            ReadLine.AutoCompletionHandler = (t, s) => t.StartsWith("git ")
                ? new[]
                {
                    "init",
                    "clone",
                    "pull",
                    "push"
                }
                : null;

            var input = ReadLine.Read("(prompt)> ");
            Console.Write(input);
        }
    }
}