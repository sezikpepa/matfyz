using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static List<int> toFactoradic(int n)
        {
            List <int> coefficients = new List <int>();
            if (n == 0)
            {
                coefficients.Add(0);
                return coefficients;
            }
            int counter = 1;
            while (n != 0)
            {
                int leftover = n % counter;
                n /= counter;
                coefficients.Add(leftover);

                counter++;
            }
            return coefficients;
        }

        static int fromFactoradic(List<int> coefficients)
        {

        }

        static void Main(string[] args)
        {
            foreach(var element in toFactoradic(0))
            {
                Console.WriteLine(element);
            }
            
        }
    }
}