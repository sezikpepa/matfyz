using System;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{

    class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    class Maze
    {

        public string[,] places;
        public int width;
        public int height;
        public Monster monster;

        public Maze(int x, int y)
        {
            this.places = new string[x, y];
            this.width = x;
            this.height = y;
        }

        public void newMonster(int x, int y, string direction)
        {
            this.monster = new Monster(x, y);
            switch (direction)
            {
                case "^":
                    monster.direction = 0;
                    break;
                case ">":
                    monster.direction = 1;
                    break;
                case "v":
                    monster.direction = 2;
                    break;
                case "<":
                    monster.direction = 3;
                    break;
            }
        }

        public void mazeStructureChange(int x, int y, string input)
        {
            if (input == "X" || input == ".")
            {
                this.places[x, y] = input;
            }
            else
            {
                newMonster(x, y, input);
            }
            
        }

        public void printMaze()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    Console.Write(this.places[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void moveMonster()
        {
            this.places[this.monster.position.x, this.monster.position.y] = ".";
            int xShift = 0;
            int yShift = 0;
            if (this.monster.direction == 0 || this.monster.direction == 2)
            {
                yShift = this.monster.direction - 1;
            }
            else if (this.monster.direction == 1 || this.monster.direction == 3)
            {
                xShift = -(this.monster.direction - 2);
            }


            if (this.places[this.monster.position.x - yShift, this.monster.position.y + xShift] == "." && !this.monster.forceForward)
            {
                this.monster.direction += 1;
                this.monster.direction %= 4;
                this.monster.forceForward = true;
            }

            else if (this.places[this.monster.position.x + xShift, this.monster.position.y + yShift] == ".")
            {
                this.monster.position.x += xShift;
                this.monster.position.y += yShift;
                this.monster.forceForward = false;
            }

            
            else if (this.places[this.monster.position.x + yShift, this.monster.position.y - xShift] == ".")
            {
                this.monster.direction -= 1;
                this.monster.direction %= 4;
            }

            else
            {
                this.monster.direction -= 1;
                this.monster.direction %= 4;
            }
            
            this.places[this.monster.position.x, this.monster.position.y] = this.monster.displayCharacter();
            
        }
    }

    class Monster
    {
        public Coordinate position;
        public int direction;
        public Dictionary<int, string> positionsRepresentation;
        public bool forceForward;

        public Monster(int x, int y)
        {
            this.position = new Coordinate(x, y);
            this.positionsRepresentation = new Dictionary<int, string>();
            this.positionsRepresentation.Add(0, "^");
            this.positionsRepresentation.Add(1, ">");
            this.positionsRepresentation.Add(2, "v");
            this.positionsRepresentation.Add(3, "<");
            this.forceForward = false;
        }

        public void normalizeDirection()
        {
            if (this.direction < 0)
            {
                this.direction = 4 + this.direction;
            }
        }

        public string displayCharacter()
        {
            normalizeDirection();
            return positionsRepresentation[this.direction];
        }
    }


    internal class Program
    {

        static void Main(string[] args)
        {
            //int width = Int32.Parse(Console.ReadLine());
            //int height = Int32.Parse(Console.ReadLine());

            int width = 10;
            int height = 6;

            Maze maze = new Maze(width, height);

            /*
            string[,] places = new string[,] { { "1", "2", "3", "4", "5", "6"},
                                               { "7", ".", ".", ".", ".", "X"},
                                               { "X", ".", ".", ".", ".", "X"},
                                               { "X", ".", "X", ".", ".", "X"},
                                               { "X", ".", "X", ".", ">", "."},
                                               { "X", "X", "X", "X", "X", "X"}};
            */
            
             string[,] places = new string[,] { { "X", "X", "X", "X", "X", "X"},
                                               { "X", ".", ".", ".", ".", "X"},
                                               { "X", ".", ".", "X", "X", "X"},
                                               { "X", ".", ".", ".", ".", "X"},
                                               { "X", ".", ".", "^", ".", "X"},
                                               { "X", "X", "X", "X", ".", "X"},
                                               { "X", ".", ".", ".", ".", "X"},
                                               { "X", ".", ".", "X", "X", "X"},
                                               { "X", ".", ".", ".", ".", "X"},
                                               { "X", "X", "X", "X", "X", "X"}};


            /*
            for (int y = 0; y < height; y++)
            {
                string newLine = Console.ReadLine();
                for (int x = 0; x < width; x++)
                {
                    maze.places[x, y] = Char.ToString(newLine[x]);
                }
            }
            */
            maze.places = places;
            maze.mazeStructureChange(4, 3, "^");
            for (int i = 0; i < 20; i++)
            {
                maze.moveMonster();
                maze.printMaze();
            }
        }
    }   
}