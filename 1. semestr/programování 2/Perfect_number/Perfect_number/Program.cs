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

        static int sum(int[] numbers)
        {
            int result = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                result += numbers[i];
            }
            return result;
        }

        static int[] findDivisors(int number)
        {
            int counter = 0;
            int[] divisors = new int[number];
            for (int i = 1; i < number; i++)
            {
                if (number % i == 0)
                {
                    divisors[counter++] = i;
                }
            }
            return divisors;
        }
        static string checkPerfect(int number)
        {
            int[] divisors = findDivisors(number);
            int sum_of_divisors = sum(divisors);
            if (sum_of_divisors == number)
            {
                return "P";
            }
            return "";
        }

        static string checkSquare(int number)
        {
            for (int i = 0; i < number / 2; ++i)
            {
                if (i * i == number)
                {
                    return "C";
                }
            }
            return "";
        }

        static string checkCube(int number)
        {
            for (int i = 0; i < number / 2; ++i)
            {
                if (i * i * i == number)
                {
                    return "K";
                }
            }
            return "";
        }


        static void Main(string[] args)
        {
            string result = "";
            int number = nextNumber();
            
            result += checkPerfect(number);
            result += checkSquare(number);
            result += checkCube(number);

            Console.WriteLine(result);

            
        }
    }
}