﻿using System;

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
                if (t.StartsWith("git"))
                    return new string[] { "git init", "git clone", "git pull", "git push", "git add" };
                else
                    return null;
            };

            string input = ReadLine.Read("(prompt)> ");
            Console.Write(input);
        }
    }
}
