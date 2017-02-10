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
            ReadLine.AddHistory(history);

            ReadLine.AutoCompletionHandler = (t, s) =>
            {
                
                string _command = t.Substring(0, t.Length);
                switch(_command)
                {
                    case "git":
                        return new string[] { "git init", "git clone", "git pull", "git push", "git add" };
                    case "ls":
                        return new string[] { "ls -a", "ls -al"};
                    case "dotnet":
                        return new string[] { "dotnet restore", "dotnet run", "dotnet new" };
                    default:
                        return null;
                }
            };

            string input = ReadLine.Read("(prompt)> ");
            Console.Write(input);
        }
    }
}
