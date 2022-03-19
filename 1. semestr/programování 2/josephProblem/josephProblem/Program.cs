using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
		static int lowestPowerOfTwo(int number)
        {
			int power = 1;
			while (power * 2 <= number)
            {
				power = power * 2;
            }
			return power;
        }
		static int Calculate_joseph_number(int soldiersNumber)
        {
			return ((soldiersNumber - lowestPowerOfTwo(soldiersNumber)) * 2 + 1);
        }
        static void Main(string[] args)
        {
			int soldiersNumber = Int32.Parse(Console.ReadLine());
			if (soldiersNumber <= 0)
            {
				Console.WriteLine("ERROR");
				return;
            }
            Console.WriteLine(Calculate_joseph_number(soldiersNumber));
			return;

        }
    }
}
/*
try:
	vstup = int(input())
except:
print("ERROR")
	exit(0)

if vstup <= 0:
	print("ERROR")
	exit(0)

power = 1
while True:
	if power * 2 <= vstup:
		power *= 2
	else:
		break


print((vstup - power) * 2 + 1)
*/
