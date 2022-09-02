using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        static int nextNumber()
        {
            int result = 0;
            bool something_found = false;
            bool negative = false;
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
                    if (negative == true)
                    {
                        return -result;
                    }
                    return result;
                }
                else if (result == -1)
                {
                    return result;
                }
                else if (character == 45)
                {
                    negative = true;
                }
            }
        }


        class Delej
        {
            Delej()
            {

            }
        }


        static void Main(string[] args)
        {
            
        }
    }
}