using System;
using System.Linq;


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

        class NumberSequence
        {
            public int[] sequence;
            public int length;
            public int[] subsequence;
            public int[] fallingSubsequence;

            public NumberSequence(int[] sequence, int sequenceLength)
            {
                this.sequence = new int[sequenceLength];
                this.sequence = sequence;

                this.subsequence = new int[sequenceLength];
                this.subsequence[0] = 1;
                this.fallingSubsequence = new int[sequenceLength];
                this.fallingSubsequence[0] = 1;

                this.length = sequenceLength;
            }

            public void draw()
            {
                for (int i = 0; i < this.sequence.Length; i++)
                {
                    Console.Write(this.sequence[i]);
                    Console.Write(" ");
                }
                Console.WriteLine();

                for (int i = 0; i < this.subsequence.Length; i++)
                {
                    Console.Write(this.subsequence[i]);
                    Console.Write(" ");
                }
                Console.WriteLine();

                for (int i = 0; i < this.fallingSubsequence.Length; i++)
                {
                    Console.Write(this.fallingSubsequence[i]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            private void fillBox(int index, int maximum)
            {
                int bestFit = 0;
                for(int i = 0; i < index; i++)
                {
                    if(this.sequence[i] <= maximum)
                    {
                        if(this.subsequence[i] > bestFit)
                        {
                            bestFit = this.subsequence[i];
                        }
                    }
                }
                this.subsequence[index] = bestFit + 1;
            }

            public void calculateSubsequence()
            {
                for(int i = 0; i < this.length; i++)
                {
                    this.fillBox(i, this.sequence[i]);
                }
            }

            public void calculateFallingSubsequence()
            {
                int maximum = this.subsequence[0];
                for (int i = 1; i < this.length; i++)
                {
                    if(this.subsequence[i - 1] > maximum)
                        maximum = this.subsequence[i - 1];

                    this.fillBoxFalling(i, this.sequence[i], maximum);
                }
            }
            private void fillBoxFalling(int index, int maximum, int candidate2)
            {
                int candidate1 = 0;
                for( int i = 0; i < index; i++)
                {
                    if(this.sequence[i] <= maximum)
                    {
                        if(this.fallingSubsequence[i] > candidate1)
                        {
                            candidate1 = this.fallingSubsequence[i];
                        }
                    }
                }
                candidate1++;
                candidate2++;

                if(candidate1 >= candidate2)
                {
                    this.fallingSubsequence[index] = candidate1;
                }
                else
                {
                    this.fallingSubsequence[index] = candidate2;
                }
            }
                               
        }


        static void Main(string[] args)
        {
            int numberCount = Int32.Parse(Console.ReadLine());

            int[] numbers = new int[numberCount];

            int number;
            for (int i = 0; i < numberCount; i++)
            {
                number = nextNumber();
                numbers[i] = number;
            }

            NumberSequence numberSequence = new NumberSequence(numbers, numberCount);

            
            numberSequence.calculateSubsequence();
            numberSequence.calculateFallingSubsequence();
            //numberSequence.draw();

            int[] result = numberSequence.fallingSubsequence;
            Console.WriteLine(result.Max());
        }
    }
}