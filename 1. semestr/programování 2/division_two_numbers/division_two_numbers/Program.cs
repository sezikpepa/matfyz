using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        static int nextNumber()
        {
            int result = 0;
            bool something_found = false;
            while (true)
            {              
                int character = Console.Read();
                if (48 <= character && 57 >= character)
                {
                    something_found = true;
                    result *= 10;
                    result += character - 48;
                }
                else if ((character == 32 || character == 13 || character == 10) && something_found == true)
                {
                    return result;
                }
                else if (result == -1)
                {
                    return result;
                }
            }
        }
        static void Main(string[] args)
        {
            int first_number = nextNumber();
            int second_number = nextNumber();

            if (second_number == 0)
            {
                Console.WriteLine("NELZE");
                return;
            }

            Console.WriteLine(first_number / second_number);
            return;
        }
    }
}