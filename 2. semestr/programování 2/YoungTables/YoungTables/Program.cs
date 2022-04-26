using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class YoungTable
        {
            public long size;
            public long[,] table;

            public YoungTable(int size)
            {
                this.size = size;
                this.table = new long[size, size];

                this.tableSetup();
                this.calculateTable();
            }

            private void tableSetup()
            {
                for(long i = 0; i < this.size; i++)
                {
                    for( long j = 0; j < this.size; j++)
                    {
                        this.table[i, j] = 0;
                    }
                    this.table[0, 0] = 1;
                }
            }

            private void calculateTable()
            {
                for(long columnIndex = 0; columnIndex < this.size; columnIndex++)
                {
                    this.calculateColumn(columnIndex);
                }
            }

            private void calculateColumn(long columnIndex)
            {
                for(long rowIndex = 0; rowIndex < this.size; rowIndex++)
                {
                    this.calculateBox(rowIndex, columnIndex);
                }
            }

            private void calculateBox(long rowIndex, long columnIndex)
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

            private long columnSum(long maxRowIndex, long columnIndex)
            {
                long sum = 0;
                for(long i = 0; i <= maxRowIndex; i++)
                {
                    sum += this.table[columnIndex, i];
                }
                return sum;
            }

            
            public long indexValue(long index)
            {
                return this.columnSum(this.size - 1, index - 1);
            }

            public void prlongTable()
            {
                for(long i = 0; i < this.size; i++)
                {
                    for(long j = 0; j < this.size; j++)
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