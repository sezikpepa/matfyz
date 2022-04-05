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
        public List<Monster> monsters; //there is a possibility I would need more monsters in the maze - this extension would be much easier this way

        public Maze(int x, int y)
        {
            this.places = new string[x, y];
            this.width = x;
            this.height = y;
            this.monsters = new List<Monster>();
        }

        public void mazeStructureChange(int x, int y, string input)
        {
            if (input == "X" || input == ".")
            {
                this.places[x, y] = input;
                return;
            }
            Monster monster = new Monster(x, y, input);
            this.monsters.Add(monster);
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

        public void moveMonsters()
        {
            foreach (Monster monster in this.monsters)
            {
                this.moveMonster(monster);
            }
        }
        public void moveMonster(Monster monster)
        {
            this.places[monster.position.x, monster.position.y] = ".";

            monster.move(this.places);
            this.displayNewMonsterState(monster);

        }

        private void displayNewMonsterState(Monster monster)
        {
            this.places[monster.position.x, monster.position.y] = monster.displayCharacter();
        }
    }

    class Monster
    {
        public Coordinate position;
        public int direction;
        private Dictionary<int, string> positionsRepresentation;
        private Dictionary<string, int> positionsRepresentationReverse;
        private bool forceForward;

        public Monster(int x, int y, string direction)
        {
            this.position = new Coordinate(x, y);
            this.positionsRepresentation = new Dictionary<int, string>();
            this.positionsRepresentation.Add(0, "^");
            this.positionsRepresentation.Add(1, ">");
            this.positionsRepresentation.Add(2, "v");
            this.positionsRepresentation.Add(3, "<");

            this.positionsRepresentationReverse = new Dictionary<string, int>();
            this.positionsRepresentationReverse.Add("^", 0);
            this.positionsRepresentationReverse.Add(">", 1);
            this.positionsRepresentationReverse.Add("v", 2);
            this.positionsRepresentationReverse.Add("<", 3);

            this.direction = this.positionsRepresentationReverse[direction];
            this.forceForward = false;
        }

        public void move(string[,] maze)
        {
            int[] shifts = new int[2];
            shifts = this.generateShiftsForNextMove();
            int xShift = shifts[0];
            int yShift = shifts[1];

            if (maze[this.position.x - yShift, this.position.y + xShift] == "." && !this.forceForward)
            {
                this.direction += 1;
                this.direction %= 4;
                this.forceForward = true;
                return;
            }

            if (maze[this.position.x + xShift, this.position.y + yShift] == ".")
            {
                this.position.x += xShift;
                this.position.y += yShift;
                this.forceForward = false;
                return;
            }


            if (maze[this.position.x + yShift, this.position.y - xShift] == ".")
            {
                this.direction -= 1;
                this.direction %= 4;
                return;
            }

            this.direction -= 1;
            this.direction %= 4;
        }


        private int[] generateShiftsForNextMove()
        {
            int xShift = 0;
            int yShift = 0;
            if (this.direction == 0 || this.direction == 2)
            {
                yShift = this.direction - 1;
            }
            else if (this.direction == 1 || this.direction == 3)
            {
                xShift = -(this.direction - 2);
            }
            int[] returnValue = new int[] { xShift, yShift };
            return returnValue;
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

        static void readInputs(Maze maze, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                string newLine = Console.ReadLine();
                for (int x = 0; x < width; x++)
                {
                    maze.mazeStructureChange(x, y, Char.ToString(newLine[x]));
                }
            }
        }

        static void moveMonsterstwentyTimes(Maze maze)
        {
            for (int i = 0; i < 20; i++)
            {
                maze.moveMonsters();
                maze.printMaze();
            }
        }

        static void Main(string[] args)
        {
            int width = Int32.Parse(Console.ReadLine());
            int height = Int32.Parse(Console.ReadLine());

            Maze maze = new Maze(width, height);

            readInputs(maze, width, height);
            moveMonsterstwentyTimes(maze);     
        }
    }
}