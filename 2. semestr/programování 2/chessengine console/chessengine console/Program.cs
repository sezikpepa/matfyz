/*
 * TODO
 * 
 * king in check
 * mat
 * 3 move repeat
 * pat
 * server play
 * engine
 * pgn load, store
 * notation
 * graphics 
 * 
 * 
 * 
 * TO FIX
 * 
 * 
 */


using System;
using System.Text;

namespace MyApp // Note: actual namespace depends on the project name.
{

    internal class Program
    {

        static public int reverseNumber8(int value)
        {
            int[] line = { 8, 7, 6, 5, 4, 3, 2, 1 };
            return line[value] - 1;
        }
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
            public Position positionEPValid;
            public int currentMove;
            public int lastEPUpdate;

            public ChessBoard()
            {
                this.playerOnMove = "white";
                this.board = new Piece[8, 8];
                this.squaresUnderAttackWhite = new bool[8, 8];
                this.squaresUnderAttackBlack = new bool[8, 8];
                this.setStartPosition();
                this.positionEPValid = new Position(-1, -1);
                this.currentMove = 1;
                this.lastEPUpdate = 0;
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

                this.board[7, 5] = new EmptySpace("blank", new Position(7, 5));
                this.board[7, 6] = new EmptySpace("blank", new Position(7, 6));

            }

            public void newEPValidPosition(int x, int y)
            {
                this.positionEPValid.x = x;
                this.positionEPValid.y = y;
                this.lastEPUpdate = this.currentMove;
            }

            public void increaseCurrentMove()
            {
                this.currentMove += 1;
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

            private bool checkCorrectPieceMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {

                this.board[rowStart, columnStart].generateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                if (this.board[rowStart, columnStart].validMoves[rowEnd, columnEnd] == false)
                    return false;
                return true;
            }

            private void checkForMarkingEPValidSquare(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                if(this.board[rowStart, columnStart].type == "pawn")
                {
                    if(Math.Abs(rowStart - rowEnd) == 2)
                    {
                        int rowIndex = (rowStart + rowEnd) / 2;
                        this.newEPValidPosition(rowIndex, columnStart);
                    }
                }
            }

            private void doShortCastle(string color)
            {
                this.increaseCurrentMove();
                
                if (color == "white")
                {
                    this.board[7, 6] = this.board[7, 4];
                    this.board[7, 5] = this.board[7, 7];

                    this.board[7, 6].updateCurrentPosition(new Position(7, 6));
                    this.board[7, 6].withoutMove = false;
                    this.board[7, 5].updateCurrentPosition(new Position(7, 5));
                    this.board[7, 5].withoutMove = false;
                    this.changePlayerOnMove();

                    this.clearSquare(7, 4);
                    this.clearSquare(7, 7);

                    return;
                }

                this.board[0, 6] = this.board[0, 4];
                this.board[0, 5] = this.board[0, 7];

                this.board[0, 6].updateCurrentPosition(new Position(0, 6));
                this.board[0, 6].withoutMove = false;
                this.board[0, 5].updateCurrentPosition(new Position(0, 5));
                this.board[0, 5].withoutMove = false;
                this.changePlayerOnMove();

                this.clearSquare(0, 4);
                this.clearSquare(0, 7);
            }

            private void doLongCastle(string color)
            {
                this.increaseCurrentMove();
                if (color == "white")
                {
                    this.board[7, 2] = this.board[7, 4];
                    this.board[7, 4] = this.board[7, 0];

                    this.clearSquare(7, 4);
                    this.clearSquare(7, 0);

                    this.board[7, 2].updateCurrentPosition(new Position(7, 2));
                    this.board[7, 2].withoutMove = false;
                    this.board[7, 4].updateCurrentPosition(new Position(7, 4));
                    this.board[7, 4].withoutMove = false;
                    this.changePlayerOnMove();

                    return;
                }
                this.board[0, 2] = this.board[0, 4];
                this.board[0, 4] = this.board[0, 0];

                this.clearSquare(0, 4);
                this.clearSquare(0, 0);

                this.board[0, 2].updateCurrentPosition(new Position(0, 2));
                this.board[0, 2].withoutMove = false;
                this.board[0, 4].updateCurrentPosition(new Position(0, 4));
                this.board[0, 4].withoutMove = false;
                this.changePlayerOnMove();
            }

            public void moveInput(Move move)
            {
                int rowStart = move.getRowIndexStartPosition();
                int rowEnd = move.getRowIndexEndPosition();
                int columnStart = move.getColumnIndexStartPosition();
                int columnEnd = move.getColumnIndexEndPosition();

                if (!this.checkMoveFromRightPlayer(rowStart, columnStart))
                    return;

                if (!this.checkCorrectPieceMove(rowStart, columnStart, rowEnd, columnEnd))
                    return;

                this.checkForMarkingEPValidSquare(rowStart, columnStart, rowEnd, columnEnd);
                
                if (this.board[rowStart, columnStart].type == "king")
                {
                    Console.WriteLine("king");
                    if (columnEnd - columnStart == 2)
                    {
                        this.doShortCastle(this.board[rowStart, columnStart].color);
                        return;
                    }
                    else if (columnEnd - columnStart == -2)
                    {
                        this.doLongCastle(this.board[rowStart, columnStart].color);
                        return;
                    }
                }
                
                this.makeMove(rowStart, columnStart, rowEnd, columnEnd);
                           
                this.board[rowEnd, columnEnd].updateCurrentPosition(new Position(rowEnd, columnEnd));
                this.board[rowEnd, columnEnd].withoutMove = false;
                this.changePlayerOnMove();

                if (rowEnd == this.positionEPValid.x && columnEnd == this.positionEPValid.y)
                {
                    if (this.board[rowEnd, columnEnd].color == "white")
                    {
                        this.board[rowEnd + 1, columnEnd] = new EmptySpace("blank", new Position(rowEnd + 1, columnEnd));
                    }
                    else if (this.board[rowEnd, columnEnd].color == "black")
                    {
                        this.board[rowEnd - 1, columnEnd] = new EmptySpace("blank", new Position(rowEnd - 1, columnEnd));
                    }
                }
                    


            }

            private void makeMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart];
                this.clearSquare(rowStart, columnStart);
                this.increaseCurrentMove();
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
                        this.board[i, j].generateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
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
                            this.board[i, j].generateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
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

            public void updateCurrentPosition(Position position)
            {
                this.position = position;
            }

            protected void linearExplore(Piece[,] board, int incrementX, int incrementY)
            {
                
                int j = this.position.y;
                int i = this.position.x;

                i += incrementY;
                j += incrementX;

                while ((0 <= i && i <= 7) && (0 <= j && j <= 7))
                {
                    if (board[i, j].color == "blank")
                    {
                        Console.WriteLine("ano");
                        this.validMoves[i, j] = true;
                    }
                    else if (board[i, j].color == this.color)
                    {
                        return;
                    }
                    else if (board[i, j].color != this.color)
                    {
                        this.validMoves[i, j] = true;
                        return;
                    }

                    i += incrementY;
                    j += incrementX;
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

            public virtual void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {

            }

            public virtual void addConsoleRepresentation()
            {

            }
        }

        public class EmptySpace : Piece
        {
            public bool EPValid;
            public EmptySpace(string color, Position position) : base(color, position)
            {
                this.EPValid = false;
                this.addConsoleRepresentation();
                this.type = "blank";
            }

            public override void addConsoleRepresentation()
            {
                this.consoleRepresentation = ' ';
            }

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
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


            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
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

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.resetValidMoves();

                this.checkMoveForward(board);
                this.checkDiscardPieceMove(board);
                this.checkMoveForwardByTwo(board);
                this.checkMoveEP(EPValidPosition, currentMove, lastEPUpdate);
                
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

            private void checkMoveEP(Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                if(currentMove - lastEPUpdate <= 1)
                {
                    if (this.position.x + increment == EPValidPosition.x && this.position.y - 1 == EPValidPosition.y)
                    {
                        this.validMoves[this.position.x + increment, this.position.y - 1] = true;
                    }
                    if (this.position.x + increment == EPValidPosition.x && this.position.y + 1 == EPValidPosition.y)
                    {
                        this.validMoves[this.position.x + increment, this.position.y + 1] = true;
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

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
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

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
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

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
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

            private bool checkKingsNextMoveNextToKing(Piece[,] board, int rowIndex, int columnIndex)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isBetweenIncluding(rowIndex + i, 0, 7) && isBetweenIncluding(columnIndex + j, 0, 7))
                        {
                            if (board[rowIndex + i, columnIndex + j].type == "king" && isOppositeColorsBW(board[rowIndex + i, columnIndex + j].color, this.color))
                            {
                                return true;
                            }
                        }
                        
                    }
                }
                return false;
            }

            private void checkCastlePossibility(Piece[,] board, int rowIndex, int columnIndex)
            {
                
                if (board[rowIndex, columnIndex].withoutMove == false)
                    return;
                //short castle
                if (board[rowIndex, columnIndex + 1].type == "blank" && board[rowIndex, columnIndex + 2].type == "blank")
                {
                    if (board[rowIndex, columnIndex + 3].withoutMove == true)
                    {
                        this.validMoves[rowIndex, columnIndex + 2] = true;
                    }
                }
                //long castle
                if (board[rowIndex, columnIndex + 1].type == "blank" && board[rowIndex, columnIndex + 2].type == "blank")
                {
                    if (board[rowIndex, columnIndex + 3].withoutMove == true)
                    {
                        this.validMoves[rowIndex, columnIndex + 2] = true;
                    }
                }

            }
         

            public override void generateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isBetweenIncluding(this.position.x + i, 0, 7) && isBetweenIncluding(this.position.y + j, 0, 7))
                        {
                            if (board[this.position.x + i, this.position.y + j].color != this.color)
                            {
                                if (!this.checkKingsNextMoveNextToKing(board, this.position.x + i, this.position.y + j))
                                {
                                    this.validMoves[this.position.x + i, this.position.y + j] = true;
                                }                            
                            }
                        }                          
                    }
                }
                this.checkCastlePossibility(board, this.position.x, this.position.y);

            }
        }

        public class Move
        {
            public string startPosition;
            public string endPosition;
            //flags
            public bool ep;
            public string castle;

            public Move(string move)
            {
                string[] parts = move.Split(' ');

                if (parts.Length != 2)
                    throw new ArgumentException("ERROR: move has to contain two parts");

                this.startPosition = parts[0];
                this.endPosition = parts[1];
                this.ep = false;
                this.castle = "none";
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
                return reverseNumber8(result);
            }

            public int getRowIndexEndPosition()
            {
                int result = this.endPosition[1] - '0' - 1;
                return reverseNumber8(result);
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
                int i = 4;
                chessBoard.board[7, i].generateValidMoves(chessBoard.board, new Position(-1, -1), 1, 1);
                chessBoard.board[7, i].drawValidMoves();

                string line = Console.ReadLine();
                Move nextMove = new(line);

                chessBoard.moveInput(nextMove);
                Console.WriteLine();

                //Console.Clear();
            }
            
        }
    }
}