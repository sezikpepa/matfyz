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
            string firstLine = Console.ReadLine();
            int numberOfNumbers = Int32.Parse(firstLine);

            int maximum = nextNumber();
            string maximumPositions = "1 ";
          

            for (int i = 1; i < numberOfNumbers; i++)
            {
                int number = nextNumber();
                if (number == maximum)
                {
                    maximumPositions += (i + 1).ToString();
                    maximumPositions += " ";
                }
                else if (number > maximum)
                {
                    maximum = number;
                    maximumPositions = (i + 1).ToString();
                    maximumPositions += " ";
                }
            }
            Console.WriteLine(maximum);
            Console.WriteLine(maximumPositions);



        }
    }
}