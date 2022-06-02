using System;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class Table
        {
            public int x;
            public int y;
            public int[,] table;
            public int[] coins;
            private int goalValue;
            private int v;

            public Table(int x, int y)
            {
                this.table = new int[x, y];
                this.coins = new int[y];
            }

            public void CalculateTable()
            {
                for(int i = 0; i < this.y; i++)
                {
                    for(int j = 0; j < this.coins.Count(); j++)
                    {
                        this.CalculateBox(i, j);
                    }
                }
            }

            public void CalculateBox(int x, int y)
            {
                int minimum = this.table[x, y];
                for(int i = 0; i < y; i++)
                {
                    if(this.table[x, i] < minimum)
                        minimum = this.table[x, i];
                }
                this.table[x, y] = minimum + 1;
            }

        }

        static void Main(string[] args)
        {
            string numberCoinsTypes = Console.ReadLine();
            int numberCoinsType = Int32.Parse(numberCoinsTypes);

            List<int> numberCoins = new List<int>();

            for(int i = 0; i < numberCoinsType; i++)
            {
                string liner = Console.ReadLine();
                int coinType = Int32.Parse(liner);

                numberCoins.Add(coinType);
            }

            string line = Console.ReadLine();
            int goalValue = Int32.Parse(line);

            //----------------------------------------------------------------------------------

            Table table = new Table(goalValue, numberCoins.Count());
            for(int i = 0; i < numberCoins.Count(); i++)
            {
                table.coins[i] = numberCoins[i];
            }
            //------------------------------------------------------------------------------------

        }
    }
}