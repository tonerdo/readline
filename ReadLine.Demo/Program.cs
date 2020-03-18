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

            IReadLine readLine = ReadLine.Instance;
            readLine.AddHistory(history);

            readLine.AutoCompletionHandler = new AutoCompletionHandler();

            var input = readLine.Read("(prompt)> ");
            Console.WriteLine(input);

            input = readLine.ReadPassword("Enter Password> ");
            Console.WriteLine(input);
        }
    }
}