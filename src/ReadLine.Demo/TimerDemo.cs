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

            while (true)
            {
                string input = ReadLine.Read("(prompt)> ");
                if (input.Length == 0)
                {
                    continue;
                }
                Console.WriteLine("You typed: \"" + input + "\"");
            }
        }
    }
}
