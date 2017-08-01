using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ReadLine Library Timer Demo");
            Console.WriteLine();

            ReadLine.InterruptInterval = 1000;
            ReadLine.CheckInterrupt = () => true;

            string initial = "";

            while (true)
            {
                ReadLine.ReadLineResult info = ReadLine.ReadExt("(prompt)> ", "", initial);
                if (info.Interrupted)
                {
                    initial = info.Result;
                    Console.WriteLine("Tick...");
                }
                else
                {
                    initial = "";
                    if (info.Result.Length != 0)
                    {
                        Console.WriteLine("You typed: \"" + info.Result + "\"");
                    }
                }
            }
        }
    }
}
