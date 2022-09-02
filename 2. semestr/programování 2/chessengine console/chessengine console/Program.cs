using System;

namespace MyApp // Note: actual namespace depends on the project name.
{

    public class Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class ChessBoard
    {
        public Piece[,] board;


        public ChessBoard()
        {
            this.board = new Piece[8, 8];
            this.setStartPosition();
        }

        public void setStartPosition()
        {
            this.board[0, 0] = new Rook("black", new Position(0, 0));
            this.board[0, 1] = new Knight("black", new Position(0, 1));
            this.board[0, 2] = new Bishop("black", new Position(0, 2));
            this.board[0, 3] = new Queen("black", new Position(0, 3));
            this.board[0, 4] = new King("black", new Position(0, 4));
            this.board[0, 5] = new Bishop("black", new Position(0, 5));
            this.board[0, 6] = new Knight("black", new Position(0, 6));
            this.board[0, 7] = new Rook("black", new Position(0, 7));

            for (int i = 0; i < 8; i++)
            {
                this.board[1, i] = new Pawn("black", new Position(1, i));
            }

            for (int i = 0; i < 8; i++)
            {
                for(int j = 2; j <= 5; j++)
                {
                    this.board[j, i] = new EmptySpace("black", new Position(j, i));
                }
            }

            for (int i = 0; i < 8; i++)
            {
                this.board[6, i] = new Pawn("white", new Position(6, i));
            }

            this.board[7, 0] = new Rook("white", new Position(7, 0));
            this.board[7, 1] = new Knight("white", new Position(7, 1));
            this.board[7, 2] = new Bishop("white", new Position(7, 2));
            this.board[7, 3] = new Queen("white", new Position(7, 3));
            this.board[7, 4] = new King("white", new Position(7, 4));
            this.board[7, 5] = new Bishop("white", new Position(7, 5));
            this.board[7, 6] = new Knight("white", new Position(7, 6));
            this.board[7, 7] = new Rook("white", new Position(7, 7));

        }

        public void consoleDraw()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Console.Write(this.board[i, j].consoleRepresentation);
                }
                Console.WriteLine();
            }
        }
    }

    public class Piece
    {
        public string color;
        public List<int []> validMoves;
        public char consoleRepresentation;
        public Position position;

        public Piece(string color, Position position)
        {
            this.checkValidityColorInput(color);
            this.consoleRepresentation = ' ';
            this.validMoves = new();
            this.position = position;
        }

        private void checkValidityColorInput(string color)
        {
            if (color == "black" || color == "white")
                this.color = color;
            else
                throw new InvalidDataException("ERROR: you have options: black, white");
        }

        public virtual void generateValidMoves(ChessBoard board)
        {

        }

        public virtual void addConsoleRepresentation()
        {

        }
    }

    public class EmptySpace : Piece
    {
        public EmptySpace(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            this.consoleRepresentation = ' ';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class Rook : Piece
    {
        public Rook(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♜';
            else
                this.consoleRepresentation = '♖';
        }

        public override void generateValidMoves(ChessBoard board)
        {
            this.validMoves.Clear();

            for (int i = this.position.x; i < 8; i++)
            {
                if (board.board[i, this.position.y].color == this.color)
                {
                    break;
                }

                if (board.board[i, this.position.y].color != this.color)
                {
                    this.validMoves.Add(new int[2] {i, this.position.y });
                    break;
                }
                else
                {
                    this.validMoves.Add(new int[2] { i, this.position.y });
                }
            }

        }
    }

    public class Pawn : Piece
    {
        public Pawn(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♟';
            else
                this.consoleRepresentation = '♙';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class Knight : Piece
    {
        public Knight(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♞';
            else
                this.consoleRepresentation = '♘';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class Bishop : Piece
    {
        public Bishop(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♝';
            else
                this.consoleRepresentation = '♗';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class Queen : Piece
    {
        public Queen(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♛';
            else
                this.consoleRepresentation = '♕';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class King : Piece
    {
        public King(string color, Position position) : base(color, position)
        {
            this.addConsoleRepresentation();
        }

        public override void addConsoleRepresentation()
        {
            if (this.color == "black")
                this.consoleRepresentation = '♚';
            else
                this.consoleRepresentation = '♔';
        }

        public override void generateValidMoves(ChessBoard board)
        {

        }
    }

    public class Move
    {
        public string startPosition;
        public string endPosition;

        public Move(string move)
        {
            string[] parts = move.Split(' ');

            if (parts.Length != 2)
                throw new ArgumentException("ERROR: move has to contain two parts");

            this.startPosition = parts[0];
            this.endPosition = parts[1];
        }

        public Move(string startPosition, string endPostion)
        {
            this.startPosition = startPosition;
            this.endPosition = endPostion;
        }

        public string getStringRepresentation()
        {
            return this.startPosition + " " + this.endPosition;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            while(true){
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                ChessBoard chessBoard = new();
                chessBoard.consoleDraw();
                string line = Console.ReadLine();
                Move nextMove = new Move(line);
            }
            
        }
    }
}