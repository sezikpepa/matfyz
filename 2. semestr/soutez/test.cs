using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("test");
            string first_line = Console.ReadLine();
            string second_line = Console.ReadLine();

            int first_number = int.Parse(first_line);
            int second_number = int.Parse(second_line);

            Console.WriteLine(first_number / second_number);
        }
    }
}