using System;
using System.Linq;

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

            ReadLine.AutoCompletionHandler = (t, s) =>
            {
                if (t.StartsWith("git "))
                    return new string[] { "init", "clone", "pull", "push", "cherry-pick", "merge", "rebase", "commit", "status" };
                else
                    return new string[] { "ls", "la", "li", "longcommandwhichwillbeononeline", "move", "moll", "moun" }.Where(x => x.StartsWith(t)).ToArray();
            };

            //string input = ReadLine.Read("(prompt)> ");
            //Console.Write(input);

            ReadLine.RollingComplete = false;
            string input = ReadLine.Read("(prompt)> ");
            Console.Write($"input was : [{input}]");
        }
    }
}
