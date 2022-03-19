using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program

        
    {
        static void printSum(string sequence)
        {
            Console.WriteLine(sequence.Substring(1));
        }
        static void decomposeToSum(int number, string alreadyMade, int startNumber)
        {
            if (number == 0)
            {
                printSum(alreadyMade);
                return;
            }
            int i = startNumber;
            while (i <= number){
                string newAlreadyMade = alreadyMade;
                newAlreadyMade += "+";
                newAlreadyMade += i;
                decomposeToSum(number - i, newAlreadyMade, i);
                i++;
            }
        }
        static void Main(string[] args)
        {
            string first_line = Console.ReadLine();
            int number = int.Parse(first_line);

            string alreadyMade = "";
            decomposeToSum(number, alreadyMade, 1);
            
        }
    }
}