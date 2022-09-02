using System;

namespace MyApp // Note: actual namespace depends on the project name.
{

    public class ChessBoard
    {
        public int[,] board;


        public ChessBoard()
        {
            this.board = new int[8, 8];
        }

        public void setStartPosition()
        {

        }
    }

    public class Piece
    {
        public string color;
        public char userRepresentation;
        public int[,] validMoves;

        public Piece(string color, char userRepresentation)
        {
            this.color = color;
            this.userRepresentation = userRepresentation;
        }

        public virtual void generateValidMoves()
        {

        }
    }

    public class Rook : Piece
    {
        public Rook(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }

    public class Pawn : Piece
    {
        public Pawn(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }

    public class Knight : Piece
    {
        public Knight(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }

    public class Bishop : Piece
    {
        public Bishop(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }

    public class Queen : Piece
    {
        public Queen(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }

    public class King : Piece
    {
        public King(string color, char userRepresentation) : base(color, userRepresentation)
        {

        }

        public override void generateValidMoves()
        {

        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            ChessBoard chessBoard = new ChessBoard();
        }
    }
}