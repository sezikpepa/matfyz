using System;


namespace Maze
{
    class MainClass
    // could be avoided
    // enables simple batch processing extension
    {
        public static void Main(string[] args)
        {
            Maze maze = new Maze();
            maze.run(20);
        }
    }

    class Maze
    {
        /*
         * Be careful data are stored in not so intuitive way!
         * int[y,x] maze:
         *   0 ------>  x
         *   | 0,0 0,1 ...
         *   | 1,0 1,1
         *   | 2,0 2,1
         *   v  .
         *      .
         *   y  .
         */

        private int width, height;
        private int[,] maze;
        private int monsterCount = 0;
        private Monster monster;
        private Monster monster2;

        // width get wrapper
        public int Width
        {
            get { return width; }
        }

        public Maze()
        {
            width = Int32.Parse(Console.ReadLine());
            height = Int32.Parse(Console.ReadLine());

            load();
        }

        public void run(int steps)
        {
            for (int i = 0; i < steps; ++i)
            {
                maze[monster.position.y, monster.position.x] = 0;
                monster.step();
                maze[monster.position.y, monster.position.x] = 1;
                maze[monster2.position.y, monster2.position.x] = 0;
                monster2.step();
                maze[monster2.position.y, monster2.position.x] = 1;
                print();
                            
            }
        }

        public void load()
        {
            maze = new int[height, width];
            // loaded character ch ~ line[j]

            for (int i = 0; i < height; ++i)
            {
                string line = Console.ReadLine();
                for (int j = 0; j < width; ++j)
                {
                    switch (line[j])
                    {
                        case '.':
                            maze[i, j] = 0;
                            break;
                        case 'X':
                            maze[i, j] = 1;
                            break;
                        case '<':
                        case '^':
                        case '>':
                        case 'v':
                            maze[i, j] = 1;
                            // create the monster
                            if(this.monsterCount == 0)
                            {
                                monster = new Monster(this, new Position(j, i), line[j]);
                                this.monsterCount++;
                            }
                            else
                            {
                                monster2 = new Monster(this, new Position(j, i), line[j]);
                            }                         
                            break;
                    }
                }

            }
        }

        public bool isWall(Position pos)
        {
            if (-1 < pos.x && pos.x < width && -1 < pos.y && pos.y < height)
            {
                if (maze[pos.y, pos.x] == 0)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        public void print()
        {
            char[] toPrint = new char[(width + 1) * height];

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (maze[i, j] == 1)
                        toPrint[i * (width + 1) + j] = 'X';
                    else
                        toPrint[i * (width + 1) + j] = '.';
                }
                toPrint[i * (width + 1) + width] = '\n';
            }

            monster.draw(ref toPrint);
            monster2.draw(ref toPrint);
            Console.WriteLine(toPrint);
        }

    }

    class Monster
    {
        // readonly is basically a ``const char[]''
        private readonly char[] directions = { '>', '^', '<', 'v' };
        public Position position;
        private int direction;
        private bool rotated;
        public Maze maze;

        public Monster(Maze maze, Position pos, int dir)
        {
            this.maze = maze;
            this.position = pos;
            this.direction = getDirection(dir);

            rotated = false;
        }

        public void step()
        {

            bool right = maze.isWall(onRight());
            bool ahead = maze.isWall(inFrontOf());

            if (ahead && right)
                rotateLeft();
            else if (!ahead && right)
                jump();
            else if (!ahead && !right)
            {
                if (rotated)
                    jump();
                else
                    rotateRight();
            }
            else // ahead && !right
                rotateRight();
        }

        private void rotateRight()
        {
            rotated = true;
            // direction - 1
            direction = (direction + 3) % 4;
        }

        private void rotateLeft()
        {
            direction = (direction + 1) % 4;
        }

        private void jump()
        {
            rotated = false;
            if (direction % 2 == 0)
                position.x += (+1 - direction);
            else
                position.y += (-2 + direction);
        }

        public void draw(ref char[] toPrint)
        {
            // short for monster
            char mos = directions[direction];

            // Width + 1 for new line character
            toPrint[position.y * (maze.Width + 1) + position.x] = mos;
        }

        public Position onRight()
        {
            if (direction % 2 == 0)
                return new Position(position.x, position.y + 1 - direction);
            else
                return new Position(position.x + 2 - direction, position.y);
        }

        public Position inFrontOf()
        {
            if (direction % 2 == 0)
                return new Position(position.x + 1 - direction, position.y);
            else
                return new Position(position.x, position.y - 2 + direction);
        }

        public int getDirection(int ch)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (directions[i] == (char)ch)
                    return i;
            }
            return 0;
        }
    }

    class Position
    {
        public int x, y;

        public Position(int x, int y)
        {
            this.x = x; this.y = y;
        }
    }
}
