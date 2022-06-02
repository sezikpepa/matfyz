using System;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class YoungTable
        {
            public int size;
            public int[,] table;
            public int[] coins;

            public YoungTable(int size, int coinSize)
            {
                this.size = size;
                this.table = new int[size, size];

                this.tableSetup();
                this.calculateTable();

                this.coins = new int[coinSize];
            }

            private void tableSetup()
            {
                for (int i = 0; i < this.size; i++)
                {
                    for (int j = 0; j < this.size; j++)
                    {
                        this.table[i, j] = 0;
                    }
                    this.table[0, 0] = 1;
                }
            }

            private void calculateTable()
            {
                for (int columnIndex = 0; columnIndex < this.size; columnIndex++)
                {
                    this.calculateColumn(columnIndex);
                }
            }

            private void calculateColumn(int columnIndex)
            {
                for (int rowIndex = 0; rowIndex < this.size; rowIndex++)
                {
                    this.calculateBox(rowIndex, columnIndex);
                }
            }

            private void calculateBox(int rowIndex, int columnIndex)
            {
                if (rowIndex == columnIndex)
                {
                    this.table[columnIndex, rowIndex] = 1;
                    return;
                }
                if (rowIndex > columnIndex)
                {
                    return;
                }
                int value = -1;

                foreach(var item in this.coins)
                {
                    if(item == rowIndex + 1)
                        value = rowIndex + 1;
                    break;
                }
                if (value == -1)
                    return;
                this.table[columnIndex, rowIndex] = this.columnSum(rowIndex, columnIndex - value) + 1;



            }

            private int columnSum(int maxRowIndex, int columnIndex)
            {
                int minimum = 2000000000;
                for (int i = 0; i <= maxRowIndex; i++)
                {
                    if(this.table[columnIndex, i] < minimum && this.table[columnIndex, i] != 0 && this.coins.Contains(columnIndex + 1))
                        minimum = this.table[columnIndex, i];
                }
                return minimum;
            }


            public int indexValue(int index)
            {
                return this.columnSum(this.size - 1, index - 1);
            }

            public void printTable()
            {
                for (int i = 0; i < this.size; i++)
                {
                    for (int j = 0; j < this.size; j++)
                    {
                        Console.Write(this.table[j, i]);
                    }
                    Console.WriteLine();
                }
            }

        }
        static void Main(string[] args)
        {
            int coinsNumber = Int32.Parse(Console.ReadLine());

            List<int> coins = new List<int>();
            

            for (int i = 0; i < coinsNumber; i++)
            {
                int value = Int32.Parse(Console.ReadLine());
                coins.Add(value);
            }
            int index = Int32.Parse(Console.ReadLine());
            YoungTable youngTable = new YoungTable(index, 3);

            for(int i = 0; i < coinsNumber; i++)
            {
                youngTable.coins[i] = coins[i];
            }
            //Console.WriteLine(youngTable.indexValue(index));
            youngTable.printTable();



        }
    }
}