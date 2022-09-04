/*
 * TODO
 * pawn e.p.
 * king castle
 * king in check
 * king next to the king - not tested
 * moves from commandline - check validity before making the move
 */


using System;
using System.Text;

namespace MyApp // Note: actual namespace depends on the project name.
{

    internal class Program
    {
       
        static public bool isBetweenIncluding(int value, int number1, int number2)
        {
            if (value <= number2 && number1 <= value)
                return true;
            return false;
        }

        static public bool isOppositeColorsBW(string color1, string color2)
        {
            if (color1 == "black" && color2 == "white")
                return true;
            if (color1 == "white" && color2 == "black")
                return true;
            return false;
        }

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
            public bool[,] squaresUnderAttackWhite;
            public bool[,] squaresUnderAttackBlack;
            public string playerOnMove;

            public ChessBoard()
            {
                this.playerOnMove = "white";
                this.board = new Piece[8, 8];
                this.squaresUnderAttackWhite = new bool[8, 8];
                this.squaresUnderAttackBlack = new bool[8, 8];
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
                    for (int j = 2; j <= 5; j++)
                    {
                        this.board[j, i] = new EmptySpace("blank", new Position(j, i));
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

            public void changePlayerOnMove()
            {
                if(this.playerOnMove == "white")
                {
                    this.playerOnMove = "black";
                }
                else
                {
                    this.playerOnMove = "white";
                }
            }

            private bool checkMoveFromRightPlayer(int rowIndex, int columnIndex)
            {
                return this.board[rowIndex, columnIndex].color == this.playerOnMove;
            }

            public void moveInput(Move move)
            {
                int rowStart = move.getRowIndexStartPosition();
                int rowEnd = move.getRowIndexEndPosition();
                int columnStart = move.getColumnIndexStartPosition();
                int columnEnd = move.getColumnIndexEndPosition();

                if (!this.checkMoveFromRightPlayer(rowStart, columnStart))
                    return;

                this.makeMove(rowStart, columnStart, rowEnd, columnEnd);
                this.changePlayerOnMove();
            }

            private void makeMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart];
                this.clearSquare(rowStart, columnStart);
            }

            public void clearSquare(int rowIndex, int columnIndex)
            {
                this.board[rowIndex, columnIndex] = new EmptySpace("blank", new Position(rowIndex, columnIndex));
            }

            public void resetSquaresUnderAttack()
            {
                this.resetSquaresUnderAttackBlack();
                this.resetSquaresUnderAttackWhite();
            }

            public void generateMovesAllPieces()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.board[i, j].generateValidMoves(this.board);
                    }
                }
            }

            public void resetSquaresUnderAttackWhite()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.squaresUnderAttackWhite[i, j] = false;
                    }
                }
            }

            public void resetSquaresUnderAttackBlack()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.squaresUnderAttackBlack[i, j] = false;
                    }
                }
            }

            public void setSquaresUnderAttackWhite()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.board[i, j].color == "white")
                        {
                            this.board[i, j].generateValidMoves(this.board);
                        }
                    }
                }
            }

            public void setSquaresUnderAttackWhiteByPiece()
            {
                this.generateMovesAllPieces();  
                this.resetSquaresUnderAttackWhite();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 0; l < 8; l++)
                            {
                                if (this.board[i, j].validMoves[k, l] == true)
                                {
                                    this.squaresUnderAttackWhite[k, l] = true;
                                }
                            }
                        }
                        
                    }
                }
            }

            public void setSquaresUnderAttackBlack()
            {

            }

            public void setSquaresUnderAttack()
            {
                this.setSquaresUnderAttackBlack();
                this.setSquaresUnderAttackWhite();
            }

            public void consoleDraw()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write(this.board[i, j].consoleRepresentation);
                    }
                    Console.WriteLine();
                }
            }

            public bool kingsNextToEachOther()
            {
                for(int k = 0; k < 8; k++)
                {
                    for(int l = 0; l < 8; l++)
                    {
                        if(this.board[k, l].type == "king")
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (isBetweenIncluding(k + i, 0, 7) && isBetweenIncluding(l + j, 0, 7))
                                    {
                                        if (board[k + i, l + j].type == "king" && (i != 0 && j != 0))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }

        }

        public class Piece
        {
            public string color;
            public bool[,] validMoves;
            public char consoleRepresentation;
            public Position position;
            public string type;
            public bool withoutMove;

            public Piece(string color, Position position)
            {
                this.checkValidityColorInput(color);
                this.consoleRepresentation = ' ';
                this.validMoves = new bool[8, 8];
                this.position = position;
                this.withoutMove = true;
                this.type = "";
            }

            protected void resetValidMoves()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.validMoves[i, j] = false;
                    }
                }
            }

            protected void linearExplore(Piece[,] board, int incrementX, int incrementY)
            {
                int j = this.position.x;
                int i = this.position.y;
                while ((0 <= i + incrementX && i + incrementX <= 7) && (0 <= j + incrementY && j + incrementY <= 7))
                {
                    if (board[i + incrementX, j + incrementY].color == "blank")
                    {
                        this.validMoves[i + incrementX, j + incrementY] = true;
                    }
                    else if (board[i + incrementX, j + incrementY].color == this.color)
                    {
                        break;
                    }
                    else
                    {
                        this.validMoves[i + incrementX, j + incrementY] = true;
                        break;
                    }

                    i += incrementX;
                    j += incrementY;
                }
            }

            public void drawValidMoves()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.validMoves[i, j] == false)
                        {
                            Console.Write('■');
                        }
                        else
                        {
                            Console.Write('□');
                        }

                    }
                    Console.WriteLine();
                }
            }

            protected void checkValidityColorInput(string color)
            {
                if (color == "black" || color == "white" || color == "blank")
                    this.color = color;
                else
                    throw new InvalidDataException("ERROR: you have options: black, white");
            }

            protected void exploreDiagonals(Piece[,] board)
            {
                this.linearExplore(board, 1, 1);
                this.linearExplore(board, -1, -1);

                this.linearExplore(board, 1, -1);
                this.linearExplore(board, -1, 1);
            }

            protected void exploreNonDiagonals(Piece[,] board)
            {
                this.linearExplore(board, 1, 0);
                this.linearExplore(board, -1, 0);

                this.linearExplore(board, 0, 1);
                this.linearExplore(board, 0, -1);
            }

            public virtual void generateValidMoves(Piece[,] board)
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
                this.type = "blank";
            }

            public override void addConsoleRepresentation()
            {
                this.consoleRepresentation = ' ';
            }

            public override void generateValidMoves(Piece[,] board)
            {

            }
        }

        public class Rook : Piece
        {
            public Rook(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "rook";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♜';
                else
                    this.consoleRepresentation = '♖';
            }


            public override void generateValidMoves(Piece[,] board)
            {
                this.resetValidMoves();

                this.exploreNonDiagonals(board);
            }
        }

        public class Pawn : Piece
        {
            public Pawn(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "pawn";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♟';
                else
                    this.consoleRepresentation = '♙';
            }

            public override void generateValidMoves(Piece[,] board)
            {
                this.resetValidMoves();

                this.checkMoveForward(board);
                this.checkDiscardPieceMove(board);
                this.checkMoveForwardByTwo(board);
                
            }

            private void checkMoveForwardByTwo(Piece[,] board)
            {
                if (this.withoutMove == false)
                    return;
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                if (board[this.position.x + increment, this.position.y].color == "blank")
                {
                    if (board[this.position.x + (increment * 2), this.position.y].color == "blank")
                    {
                        this.validMoves[this.position.x + (increment * 2), this.position.y] = true;
                    }
                }              
            }


            private void checkMoveForward(Piece[,] board)
            {
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                if (board[this.position.x + increment, this.position.y].color == "blank")
                    this.validMoves[this.position.x + increment, this.position.y] = true;
            }

            private void checkDiscardPieceMove(Piece[,] board)
            {
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                try
                {
                    if (isOppositeColorsBW(board[this.position.x + increment, this.position.y + 1].color, this.color))
                        this.validMoves[this.position.x + increment, this.position.y + 1] = true;
                }
                catch
                {

                }

                try
                {
                    if (isOppositeColorsBW(board[this.position.x + increment, this.position.y - 1].color, this.color))
                        this.validMoves[this.position.x + increment, this.position.y - 1] = true;
                }
                catch
                {

                }

               
            }
        }

        public class Knight : Piece
        {
            public Knight(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "knight";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♞';
                else
                    this.consoleRepresentation = '♘';
            }

            public override void generateValidMoves(Piece[,] board)
            {
                this.resetValidMoves();
                int x = this.position.x;
                int y = this.position.y;
                for (int i = -2; i <= 2; i++)
                {
                    for(int j = -2; j <= 2; j++)
                    {
                        if(isBetweenIncluding(x - i, 0, 7) && isBetweenIncluding(y - j, 0, 7) && Math.Abs(i) + Math.Abs(j) == 3 && i != 0 && j != 0)
                        {
                            if (board[x - i, y - j].color != this.color)
                                this.validMoves[x - i, y - j] = true;
                        }
                    }
                }
                
            }
        }

        public class Bishop : Piece
        {
            public Bishop(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "bishop";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♝';
                else
                    this.consoleRepresentation = '♗';
            }

            public override void generateValidMoves(Piece[,] board)
            {
                this.resetValidMoves();

                this.exploreDiagonals(board);


            }
        }

        public class Queen : Piece
        {
            public Queen(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "queen";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♛';
                else
                    this.consoleRepresentation = '♕';
            }

            public override void generateValidMoves(Piece[,] board)
            {
                this.resetValidMoves();

                this.exploreDiagonals(board);
                this.exploreNonDiagonals(board);
            }
        }

        public class King : Piece
        {
            public King(string color, Position position) : base(color, position)
            {
                this.addConsoleRepresentation();
                this.type = "king";
            }

            public override void addConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♚';
                else
                    this.consoleRepresentation = '♔';
            }

            public override void generateValidMoves(Piece[,] board)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isBetweenIncluding(this.position.x + i, 0, 7) && isBetweenIncluding(this.position.y + j, 0, 7))
                        {
                            if (board[this.position.x + i, this.position.y + j].color != this.color)
                            {
                                this.validMoves[this.position.x + i, this.position.y + j] = true;
                            }
                        }                          
                    }
                }
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

            private int getColumnIndex(string value)
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
                return asciiBytes[0] - 97;
            }

            public int getColumnIndexStartPosition()
            {
                return this.getColumnIndex(this.startPosition);
            }

            public int getColumnIndexEndPosition()
            {
                return this.getColumnIndex(this.endPosition);
            }

            public int getRowIndexStartPosition()
            {
                int result = this.startPosition[1] - '0' - 1;
                return result;
            }

            public int getRowIndexEndPosition()
            {
                int result = this.endPosition[1] - '0' - 1;
                return result;
            }

            public string getStringRepresentation()
            {
                return this.startPosition + " " + this.endPosition;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            ChessBoard chessBoard = new();
            while (true){              
                chessBoard.consoleDraw();
               
                string line = Console.ReadLine();
                Move nextMove = new(line);

                chessBoard.moveInput(nextMove);
                Console.WriteLine();

            }
            
        }
    }
}