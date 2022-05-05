using System;
using System.Collections.Generic;

namespace MyApp
{
    internal class Program
    {

        static void FindPath(char[,] chessboard, (int x, int y, int length) start, int width, int height)
        {
            Queue<(int x, int y, int length)> positions = new();
            positions.Enqueue(start);
            chessboard[start.x, start.y] = 'X';
            while (positions.Count > 0)
            {
                (int x, int y, int length) element = positions.Dequeue();
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {

                        if (element.x + i < width && element.y + j < height && 0 <= element.x + i && 0 <= element.y + j && chessboard[element.x + i, element.y + j] != 'X')
                        {
                            if (chessboard[element.x + i, element.y + j] == 'C')
                            {
                                Console.WriteLine(element.length + 1);
                                return;
                            }
                            positions.Enqueue((element.x + i, element.y + j, element.length + 1));
                            chessboard[element.x + i, element.y + j] = 'X';
                        }
                    }
                }
            }
            Console.WriteLine(-1);
            return;


        }
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("./sachovnice.txt");

            int width = int.Parse(lines[1]);
            int height = int.Parse(lines[0]);

            (int x, int y, int length) start;
            start.x = 0;
            start.y = 0;
            start.length = 0;

            char[,] chessboard = new char[width, height];

            for (int i = 2; i < height + 2; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    string current = lines[i];
                    if (current[j] == 'S')
                    {
                        start.x = j;
                        start.y = i - 2;
                    }
                    chessboard[j, i - 2] = current[j];
                }
            }

            FindPath(chessboard, start, width, height);
        }
    }
}