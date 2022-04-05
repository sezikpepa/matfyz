using System;

namespace MyApp
{
    class Board
    {
        public string[,] fields;

        public Board(string[,] fields)
        {
            this.fields = new string[3,3];
            this.fields = fields;
        }

        public void print()
        {
            for (int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    Console.Write(this.fields[x,y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Player
    {
        public string symbol;

        public Player(string symbol)
        {
            this.symbol = symbol;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}