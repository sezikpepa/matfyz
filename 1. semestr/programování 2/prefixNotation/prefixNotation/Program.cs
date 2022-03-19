using System;
using System.Collections;
using System.Linq;
namespace MyApp 
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
        static int calculate(int number1, int number2, string mathematicalOperator)
        {
            if (mathematicalOperator == "+")
            {
                return number1 + number2;
            }
            else if (mathematicalOperator == "-")
            {
                return number1 - number2;
            }
            else if (mathematicalOperator == "-*")
            {
                return number1 * number2;
            }
            else if (mathematicalOperator == "/")
            {
                return number1 / number2;
            }
            return 0;
        }

        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            string[] splittedLine = line.Split(" ");
            Stack<string> mathematicalOperators = new Stack<string>();
            Stack<int> numbers = new Stack<int>();

            string operators = "+-*/";
            foreach (string element in splittedLine)
            {
                if (operators.Contains(element))
                {
                    mathematicalOperators.Push(element);
                }
            }
             
            
            


        }
    }
}