using System;
using System.Collections;
using System.Linq;
namespace MyApp
{
    internal class Program
    {

        static string nextElement()
        {
            string result = "";
            while (true)
            {
                char character = Convert.ToChar(Console.Read());
                if (48 <= character && 57 >= character)
                {
                    result += character;
                }
                else if (character <= 32 && result != "")
                {
                    return result;
                }
                else if (character == 43 || character == 45 || character == 42 || character == 47)
                {
                    return character.ToString();
                }
                
            }
        }

        static int CalculatePrefixNotation()
        {
            string nextPart = nextElement();
            try
            {
                int number = int.Parse(nextPart);
                return number;
            }
            catch
            {
                int part1 = CalculatePrefixNotation();
                int part2 = CalculatePrefixNotation();
                if (nextPart == "+")
                {
                    return part1 + part2;
                }

                else if (nextPart == "-")
                {
                    return part1 - part2;
                }
                else if (nextPart == "*")
                {
                    return part1 * part2;
                }
                else
                {
                    return part1 / part2;
                }
            } 

        }

        static void Main(string[] args)
        {
            int result = CalculatePrefixNotation();

            Console.WriteLine(result);





        }
    }
}