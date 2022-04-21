using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class YoungTable
        {
            public int size;
            public int[,] table;

            public YoungTable(int size)
            {
                this.size = size;
                this.table = new int[size, size];

                this.tableSetup();
                this.calculateTable();
            }

            private void tableSetup()
            {
                for(int i = 0; i < this.size; i++)
                {
                    for( int j = 0; j < this.size; j++)
                    {
                        this.table[i, j] = 0;
                    }
                    this.table[0, 0] = 1;
                }
            }

            private void calculateTable()
            {
                for(int columnIndex = 0; columnIndex < this.size; columnIndex++)
                {
                    this.calculateColumn(columnIndex);
                }
            }

            private void calculateColumn(int columnIndex)
            {
                for(int rowIndex = 0; rowIndex < this.size; rowIndex++)
                {
                    this.calculateBox(rowIndex, columnIndex);
                }
            }

            private void calculateBox(int rowIndex, int columnIndex)
            {
                if(rowIndex == columnIndex)
                {
                    this.table[columnIndex, rowIndex] = 1;
                    return;
                }
                if(rowIndex > columnIndex)
                {
                    return;
                }
                this.table[columnIndex, rowIndex] = this.columnSum(rowIndex, columnIndex - (rowIndex + 1));
            }

            private int columnSum(int maxRowIndex, int columnIndex)
            {
                int sum = 0;
                for(int i = 0; i <= maxRowIndex; i++)
                {
                    sum += this.table[columnIndex, i];
                }
                return sum;
            }

            
            public int indexValue(int index)
            {
                return this.columnSum(this.size - 1, index - 1);
            }

            public void printTable()
            {
                for(int i = 0; i < this.size; i++)
                {
                    for(int j = 0; j < this.size; j++)
                    {
                        Console.Write(this.table[j, i]);
                    }
                    Console.WriteLine();
                }
            }

        }




        static void Main(string[] args)
        {
            int index = Int32.Parse(Console.ReadLine());
            if (index == 0)
            {
                Console.WriteLine("1");
            }
            else if (index < 0)
            {
                Console.WriteLine("0");
            }
            else
            {
                YoungTable youngTable = new YoungTable(index);
                Console.WriteLine(youngTable.indexValue(index));
            }
            
            
            

            

          
        }
    }
}