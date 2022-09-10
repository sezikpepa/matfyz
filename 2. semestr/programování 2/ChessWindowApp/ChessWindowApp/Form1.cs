using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChessWindowApp
{

    public partial class Form1 : Form
    {
        public int mode = 0; //0: jen tak si tahat, 1: internet 2, engine
        static public string? playerColor;
        public static string infoForMoveInput = "";
        static public bool stop = false;
        ChessBoard chessBoard = new();
        public Button[,] brnGrid = new Button[8, 8];
        public Label[,] whiteDiscardedPiecesLabels = new Label[5, 2];
        public Label[,] blackDiscardedPiecesLabels = new Label[5, 2];

        public bool playAsWhite;

        public Communicator onlineCommunicator;


        private Button? lastClickedButton;

        public Position startPosition;
        public Position endPosition;

        public bool showValidMoves;

        public Form1()
        {
            this.playAsWhite = true;

            

            InitializeComponent();
            this.Text = "MatfyzBot 1.0";
            this.showValidMoves = false;

            this.ResetPositions();
            this.lastClickedButton = null;

            this.PrintButtonGrid();
            this.PrintDiscardedPieces();

            //this.redrawBoardWhiteTop();
        }

        private void CheckMoveFromInternet(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = (System.Windows.Forms.Timer)sender;
            if (timer.Tag.ToString() != "internetTimer") return;

            if (infoForMoveInput != "")
            {
                if (infoForMoveInput == "00")
                {
                    this.DisableChessGrid();
                    
                    infoForMoveInput = "";
                    this.actualizationMoveFromServerTimer.Enabled = false;
                    this.actualizationMoveFromServerTimer.Stop();
                    this.actualizationMoveFromServerTimer = null;
                    this.onlineCommunicator = null;
                    stop = true;
                    timer.Tag = "stop";
                    if (this.gameInfoLabel.Text == "Opponent resign") return;
                    MessageBox.Show("Opponent resign");
                    this.gameInfoLabel.Text = "Opponent resign";

                    return;
                }
                else
                {
                    this.chessBoard.MoveInput(new Move(infoForMoveInput));
                    this.RedrawChessGrid();
                    infoForMoveInput = "";

                    this.DisableChessGrid();
                    return;
                }
            }

            if (playerColor != this.chessBoard.playerOnMove)
            {
                this.DisableChessGrid();
                return;
            }
            else
            {
                this.EnableChessGrid();
            }

        }

        private void ResignButtonClicked(object sender, EventArgs e)
        {
            this.onlineCommunicator.SendResign();
            this.DisableChessGrid();
        }

        private void ResetPositions()
        {
            this.startPosition = new Position(-1, -1);
            this.endPosition = new Position(-1, -1);
        }

        private void PrintButtonGrid()
        {
            int buttonSize = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    brnGrid[i, j] = new Button
                    {
                        Width = buttonSize,
                        Height = buttonSize
                    };

                    brnGrid[i, j].Click += this.GridButtonClick;

                    panel1.Controls.Add(brnGrid[i, j]);

                    brnGrid[i, j].Location = new Point(i * buttonSize, j * buttonSize);

                    if(this.playAsWhite == true)
                        brnGrid[i, j].Text = Char.ToString(chessBoard.board[j, i].consoleRepresentation);
                    else
                        brnGrid[i, j].Text = Char.ToString(chessBoard.board[7 - j, i].consoleRepresentation);

                    Font chessPieceFont = new("Arial", 50);
                    brnGrid[i, j].Font = chessPieceFont;

                    brnGrid[i, j].Tag = new Point(i, j);

                    this.SetButtonOriginalColor(brnGrid[i, j]);
                }
            }
        }

        private void PrintDiscardedPieces()
        {
            this.PrintDiscardedPiecesWhite();
            this.PrintDiscardedPiecesBlack();
        }

        private void PrintDiscardedPiecesWhite()
        {
            
            int iterable = 0;
            foreach(var element in this.chessBoard.discardedPiecesWhite)
            {
                int labelSize = 50;
                int labelSize2 = 50;

                this.whiteDiscardedPiecesLabels[iterable, 0] = new Label();
                this.whiteDiscardedPiecesLabels[iterable, 1] = new Label();

                this.whiteDiscardedPiecesLabels[iterable, 0].Text = element.Key.ToString();
                this.whiteDiscardedPiecesLabels[iterable, 1].Text = element.Value.ToString();

                this.whiteDiscardedPiecesLabels[iterable, 0].Height = labelSize;
                this.whiteDiscardedPiecesLabels[iterable, 1].Height = labelSize;

                this.whiteDiscardedPiecesLabels[iterable, 0].Width = labelSize2;
                this.whiteDiscardedPiecesLabels[iterable, 1].Width = labelSize2;

                this.whiteDiscardedPiecesLabels[iterable, 0].Location = new Point(0, 0 + iterable * labelSize);
                this.whiteDiscardedPiecesLabels[iterable, 1].Location = new Point(80, 13 + iterable * labelSize);

                Font discardedPiecesFont = new("Arial", 30);
                Font discardedPiecesNumbersFont = new("Arial", 20);
                this.whiteDiscardedPiecesLabels[iterable, 0].Font = discardedPiecesFont;
                this.whiteDiscardedPiecesLabels[iterable, 1].Font = discardedPiecesNumbersFont;

                this.whiteDiscardedPiecesPanel.Controls.Add(this.whiteDiscardedPiecesLabels[iterable, 0]);
                this.whiteDiscardedPiecesPanel.Controls.Add(this.whiteDiscardedPiecesLabels[iterable, 1]);

                iterable += 1;
            }
        }

        private void PrintDiscardedPiecesBlack()
        {
            int iterable = 0;
            int labelSize = 50;
            int labelSize2 = 50;

            foreach (var element in this.chessBoard.discardedPiecesWhite)
            {

                this.blackDiscardedPiecesLabels[iterable, 0] = new Label();
                this.blackDiscardedPiecesLabels[iterable, 1] = new Label();

                this.blackDiscardedPiecesLabels[iterable, 0].Text = element.Key.ToString();
                this.blackDiscardedPiecesLabels[iterable, 1].Text = element.Value.ToString();

                this.blackDiscardedPiecesLabels[iterable, 0].Height = labelSize;
                this.blackDiscardedPiecesLabels[iterable, 1].Height = labelSize;

                this.blackDiscardedPiecesLabels[iterable, 0].Width = labelSize2;
                this.blackDiscardedPiecesLabels[iterable, 1].Width = labelSize2;

                this.blackDiscardedPiecesLabels[iterable, 0].Location = new Point(0, 0 + iterable * labelSize);
                this.blackDiscardedPiecesLabels[iterable, 1].Location = new Point(80, 13 + iterable * labelSize);

                Font discardedPiecesFont = new("Arial", 30);
                Font discardedPiecesNumbersFont = new("Arial", 20);
                this.blackDiscardedPiecesLabels[iterable, 0].Font = discardedPiecesFont;
                this.blackDiscardedPiecesLabels[iterable, 1].Font = discardedPiecesNumbersFont;

                this.blackDiscardedPiecesPanel.Controls.Add(this.blackDiscardedPiecesLabels[iterable, 0]);
                this.blackDiscardedPiecesPanel.Controls.Add(this.blackDiscardedPiecesLabels[iterable, 1]);

                iterable += 1;
            }
        }

        public void RedrawChessGrid()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    brnGrid[i, j].Text = Char.ToString(chessBoard.board[j, i].consoleRepresentation);
                    Font chessPieceFont = new("Arial", 50);
                    brnGrid[i, j].Font = chessPieceFont;

                    brnGrid[i, j].Tag = new Point(i, j);
                }
            }
        }

        public void RedrawBoardWhiteTop()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    this.brnGrid[i, j].Text = Char.ToString(chessBoard.board[7 - j, i].consoleRepresentation);
                }
            }
        }

        

        private void SetButtonOriginalColor(Button button)
        {
            if (button == null)
                return;

            Point tag = (Point) button.Tag;

            if((tag.X + tag.Y) % 2 == 0)
            {
                button.BackColor = Color.White;
                return;
            }
            button.BackColor = Color.Brown;
        }

        public void SetAllButtonOriginalColor()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.SetButtonOriginalColor(brnGrid[i, j]);
                }
            }
        }

        public void HideValidMoves()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this.startPosition != null)
                    {
                        if (this.startPosition.x == i && this.startPosition.y == j)
                        {
                            continue;
                        }
                    }
                    this.SetButtonOriginalColor(brnGrid[i, j]);
                }
            }
        }

        private void GridButtonClick(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (this.lastClickedButton != null)
                    this.SetButtonOriginalColor(this.lastClickedButton);

                Button clickedButton = (Button)sender;
                Point location = (Point)clickedButton.Tag;
                if (!this.startPosition.IsValid())
                {
                    if(this.chessBoard.board[location.Y, location.X].color == this.chessBoard.playerOnMove)
                    {
                        this.startPosition = new Position(location.X, location.Y);
                        clickedButton.BackColor = Color.LightBlue;
                        this.chessBoard.board[location.Y, location.X].GenerateValidMoves(this.chessBoard.board, this.chessBoard.positionEPValid, this.chessBoard.currentMove, this.chessBoard.lastEPUpdate);
                        if (this.showValidMoves)
                        {
                            this.ShowUserValidSquares(this.chessBoard.board[location.Y, location.X].validMoves);
                        }
                    }
                    
                }
                else
                {
                    this.endPosition = new Position(location.X, location.Y);
                    this.GuiMoveInput(startPosition, endPosition);

                    this.SetAllButtonOriginalColor();
                    this.ResetPositions();
                }

                this.lastClickedButton = clickedButton;
                this.RedrawChessGrid();
            }

            this.SetValuesDiscardePiecesLabels();
            
            
        }

        private void SetValuesDiscardePiecesLabels()
        {
            string[] list = { "♕", "♖", "♗", "♘", "♙" };
            string[] list2 = { "♛", "♜", "♝", "♞", "♟" };
            for (int i = 0; i < 5; i++)
            {
                this.whiteDiscardedPiecesLabels[4 - i, 1].Text = this.chessBoard.discardedPiecesWhite[list[i]].ToString();
            }
            for (int i = 0; i < 5; i++)
            {
                this.blackDiscardedPiecesLabels[4 - i, 1].Text = this.chessBoard.discardedPiecesBlack[list2[i]].ToString();
            }
        }

        public void ShowUserValidSquares(bool[,] validMoves)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(validMoves[i, j] == true)
                    {
                        if (this.chessBoard.board[i, j].type == "blank")
                        {
                            this.brnGrid[j, i].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            this.brnGrid[j, i].BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void GuiMoveInput(Position startPosition, Position endPosition)
        {
            Move move = new(startPosition, endPosition);

            chessBoard.MoveInput(move);

            if (this.mode == 1)
                this.onlineCommunicator.SendString(move.GetStringRepresentation());

            this.RedrawChessGrid();

        }

        private void ChoosePlayerColorCheckBoxChanged(object sender, EventArgs e)
        {

        }

        private void DisableChessGrid()
        {
            this.ChangeChessGridDisable(false);
        }

        private void EnableChessGrid()
        {
            this.ChangeChessGridDisable(true);
        }

        private void ChangeChessGridDisable(bool enable)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.brnGrid[i, j].Enabled = enable;
                }
            }
        }

        private void ChooseOpponentComboBoxValueChanged(object sender, EventArgs e)
        {
            ComboBox choosePlayerComboBox = (ComboBox)sender;

            this.SetOpponentNameLabel(choosePlayerComboBox.Text);

            if (choosePlayerComboBox.Text == "Náhodný člověk z internetu")
            {
                this.onlineCommunicator = new();
                this.mode = 1;
                this.actualizationMoveFromServerTimer.Start();
                MessageBox.Show("žeby tady byl problém?");
            }
            if (choosePlayerComboBox.Text == "Jen tak si tahat")
            {
                this.mode = 0;
                this.actualizationMoveFromServerTimer.Stop();
            }

        }

        private void SetOpponentNameLabel(string text)
        {
            if(text == "Jen tak si tahat")
            {
                this.opponentNameLabel.Text = "You";
            }
            this.opponentNameLabel.Text = text;
        }

        private void ShowValidMovesCheckBoxChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                this.showValidMoves = true;
                return;
            }
            this.showValidMoves = false;
            this.HideValidMoves();

        }

        private void ResetButtonClicked(object sender, EventArgs e)
        {
            this.chessBoard = new ChessBoard();
            this.RedrawChessGrid();
            this.ResetPositions();

            this.chessBoard.ResetDiscardedPieces();

            this.SetAllButtonOriginalColor();
            this.SetValuesDiscardePiecesLabels();

            this.EnableChessGrid();
        }

        static public int ReverseNumber8(int value)
        {
            if (value < 0 || value > 7)
            {
                return 0;
            }
            int[] line = { 8, 7, 6, 5, 4, 3, 2, 1};
            return line[value] - 1;
        }
        static public bool IsBetweenIncluding(int value, int number1, int number2)
        {
            return value <= number2 && number1 <= value;
        }

        static public bool IsOppositeColorsBW(string color1, string color2)
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

            public bool IsValid()
            {
                return (IsBetweenIncluding(this.x, 0, 7) && IsBetweenIncluding(this.y, 0, 7));
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
            public bool toResetUserSigns;

            public Dictionary<string, int> discardedPiecesWhite;
            public Dictionary<string, int> discardedPiecesBlack;

            public ChessBoard()
            {
                this.playerOnMove = "white";
                this.board = new Piece[8, 8];
                this.squaresUnderAttackWhite = new bool[8, 8];
                this.squaresUnderAttackBlack = new bool[8, 8];
                this.SetStartPosition();
                this.positionEPValid = new Position(-1, -1);
                this.currentMove = 1;
                this.lastEPUpdate = 0;
                this.toResetUserSigns = false;

                this.ResetDiscardedPieces();
            }

            public void SetStartPosition()
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

            public void ResetDiscardedPieces()
            {
                this.ResetDiscardedPiecesWhite();
                this.ResetDiscardedPiecesBlack();
            }

            private void ResetDiscardedPiecesWhite()
            {
                this.discardedPiecesWhite = new Dictionary<string, int>
                {
                    { "♙", 0 },
                    { "♘", 0 },
                    { "♗", 0 },
                    { "♖", 0 },
                    { "♕", 0 }
                };

            }

            private void ResetDiscardedPiecesBlack()
            {
                this.discardedPiecesBlack = new Dictionary<string, int>
                {
                    { "♟", 0 },
                    { "♞", 0 },
                    { "♝", 0 },
                    { "♜", 0 },
                    { "♛", 0 }
                };

            }

            public void NewEPValidPosition(int x, int y)
            {
                this.positionEPValid.x = x;
                this.positionEPValid.y = y;
                this.lastEPUpdate = this.currentMove;
            }

            public void IncreaseCurrentMove()
            {
                this.currentMove += 1;
            }

            public void ChangePlayerOnMove()
            {
                if (this.playerOnMove == "white")
                {
                    this.playerOnMove = "black";
                }
                else
                {
                    this.playerOnMove = "white";
                }
            }

            private bool CheckMoveFromRightPlayer(int rowIndex, int columnIndex)
            {
                return this.board[rowIndex, columnIndex].color == this.playerOnMove;
            }

            private bool CheckCorrectPieceMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {

                this.board[rowStart, columnStart].GenerateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                if (this.board[rowStart, columnStart].validMoves[rowEnd, columnEnd] == false)
                    return false;
                return true;
            }

            private void CheckForMarkingEPValidSquare(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                if (this.board[rowStart, columnStart].type == "pawn")
                {
                    if (Math.Abs(rowStart - rowEnd) == 2)
                    {
                        int rowIndex = (rowStart + rowEnd) / 2;
                        this.NewEPValidPosition(rowIndex, columnStart);
                    }
                }
            }

            private void DoShortCastle(string color)
            {
                this.IncreaseCurrentMove();

                if (color == "white")
                {
                    this.board[7, 6] = this.board[7, 4];
                    this.board[7, 5] = this.board[7, 7];

                    this.board[7, 6].UpdateCurrentPosition(new Position(7, 6));
                    this.board[7, 6].withoutMove = false;
                    this.board[7, 5].UpdateCurrentPosition(new Position(7, 5));
                    this.board[7, 5].withoutMove = false;
                    this.ChangePlayerOnMove();

                    this.ClearSquare(7, 4);
                    this.ClearSquare(7, 7);

                    return;
                }

                this.board[0, 6] = this.board[0, 4];
                this.board[0, 5] = this.board[0, 7];

                this.board[0, 6].UpdateCurrentPosition(new Position(0, 6));
                this.board[0, 6].withoutMove = false;
                this.board[0, 5].UpdateCurrentPosition(new Position(0, 5));
                this.board[0, 5].withoutMove = false;
                this.ChangePlayerOnMove();

                this.ClearSquare(0, 4);
                this.ClearSquare(0, 7);
            }

            private void DoLongCastle(string color)
            {
                this.IncreaseCurrentMove();
                if (color == "white")
                {
                    this.board[7, 2] = this.board[7, 4];
                    this.board[7, 3] = this.board[7, 0];

                    this.ClearSquare(7, 4);
                    this.ClearSquare(7, 0);

                    this.board[7, 2].UpdateCurrentPosition(new Position(7, 2));
                    this.board[7, 2].withoutMove = false;
                    this.board[7, 3].UpdateCurrentPosition(new Position(7, 3));
                    this.board[7, 3].withoutMove = false;
                    this.ChangePlayerOnMove();

                    return;
                }
                this.board[0, 2] = this.board[0, 4];
                this.board[0, 3] = this.board[0, 0];

                this.ClearSquare(0, 4);
                this.ClearSquare(0, 0);

                this.board[0, 2].UpdateCurrentPosition(new Position(0, 2));
                this.board[0, 2].withoutMove = false;
                this.board[0, 3].UpdateCurrentPosition(new Position(0, 3));
                this.board[0, 3].withoutMove = false;
                this.ChangePlayerOnMove();
            }

            public void MoveInput(Move move)
            {
                int rowStart = move.GetRowIndexStartPosition();
                int rowEnd = move.GetRowIndexEndPosition();
                int columnStart = move.GetColumnIndexStartPosition();
                int columnEnd = move.GetColumnIndexEndPosition();

                if (!this.CheckMoveFromRightPlayer(rowStart, columnStart))
                    return;

                if (!this.CheckCorrectPieceMove(rowStart, columnStart, rowEnd, columnEnd))
                    return;

                //if (this.isKingChecked(this.board, rowEnd, columnEnd))
                //return;

                this.CheckForMarkingEPValidSquare(rowStart, columnStart, rowEnd, columnEnd);

                if (this.board[rowStart, columnStart].type == "king")
                {
                    Console.WriteLine("king");
                    if (columnEnd - columnStart == 2)
                    {
                        this.DoShortCastle(this.board[rowStart, columnStart].color);

                        return;
                    }
                    else if (columnEnd - columnStart == -2)
                    {
                        this.DoLongCastle(this.board[rowStart, columnStart].color);
                        return;
                    }
                }

                this.MakeMove(rowStart, columnStart, rowEnd, columnEnd);

                this.board[rowEnd, columnEnd].UpdateCurrentPosition(new Position(rowEnd, columnEnd));
                this.board[rowEnd, columnEnd].withoutMove = false;
                this.ChangePlayerOnMove();

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

            private void MakeMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                if (this.board[rowEnd, columnEnd].type != "blank")
                {
                    this.ChangeDiscardedPiecesCount(this.board[rowEnd, columnEnd]);
                }
                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart];
                this.ClearSquare(rowStart, columnStart);
                this.IncreaseCurrentMove();
            }

            private void ChangeDiscardedPiecesCount(Piece piece)
            {
                if (piece == null)
                    return;

                if (piece.color == "white")
                {
                    this.discardedPiecesWhite[piece.consoleRepresentation.ToString()] += 1;
                }
                else if (piece.color == "black")
                {
                    this.discardedPiecesBlack[piece.consoleRepresentation.ToString()] += 1;
                }
            }

            public void ClearSquare(int rowIndex, int columnIndex)
            {
                this.board[rowIndex, columnIndex] = new EmptySpace("blank", new Position(rowIndex, columnIndex));
            }

            public bool IsKingChecked(Piece[,] board, int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[i, j].GenerateValidMoves(board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                    }
                }

                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart];
                return false;
            }

            public void ResetSquaresUnderAttack()
            {
                this.ResetSquaresUnderAttackBlack();
                this.ResetSquaresUnderAttackWhite();
            }

            public void GenerateMovesAllPieces()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.board[i, j].GenerateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                    }
                }
            }

            public void ResetSquaresUnderAttackWhite()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.squaresUnderAttackWhite[i, j] = false;
                    }
                }
            }

            public void ResetSquaresUnderAttackBlack()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.squaresUnderAttackBlack[i, j] = false;
                    }
                }
            }

            public void SetSquaresUnderAttackWhite()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.board[i, j].color == "white")
                        {
                            this.board[i, j].GenerateValidMoves(this.board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                        }
                    }
                }
            }

            public void SetSquaresUnderAttackWhiteByPiece()
            {
                this.GenerateMovesAllPieces();
                this.ResetSquaresUnderAttackWhite();
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

            public void SetSquaresUnderAttackBlack()
            {

            }

            public void SetSquaresUnderAttack()
            {
                this.SetSquaresUnderAttackBlack();
                this.SetSquaresUnderAttackWhite();
            }

            public void ConsoleDraw()
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

            public bool KingsNextToEachOther()
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int l = 0; l < 8; l++)
                    {
                        if (this.board[k, l].type == "king")
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (IsBetweenIncluding(k + i, 0, 7) && IsBetweenIncluding(l + j, 0, 7))
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
                this.CheckValidityColorInput(color);
                this.consoleRepresentation = ' ';
                this.validMoves = new bool[8, 8];
                this.position = position;
                this.withoutMove = true;
                this.type = "";
                this.color = color;
            }

            protected void ResetValidMoves()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        this.validMoves[i, j] = false;
                    }
                }
            }

            public void UpdateCurrentPosition(Position position)
            {
                this.position = position;
            }

            protected void LinearExplore(Piece[,] board, int incrementX, int incrementY)
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

            public void DrawValidMoves()
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

            protected void CheckValidityColorInput(string color)
            {
                if (color == "black" || color == "white" || color == "blank")
                    this.color = color;
                else
                    throw new InvalidDataException("ERROR: you have options: black, white");
            }

            protected void ExploreDiagonals(Piece[,] board)
            {
                this.LinearExplore(board, 1, 1);
                this.LinearExplore(board, -1, -1);

                this.LinearExplore(board, 1, -1);
                this.LinearExplore(board, -1, 1);
            }

            protected void ExploreNonDiagonals(Piece[,] board)
            {
                this.LinearExplore(board, 1, 0);
                this.LinearExplore(board, -1, 0);

                this.LinearExplore(board, 0, 1);
                this.LinearExplore(board, 0, -1);
            }

            public virtual void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {

            }

            public virtual void AddConsoleRepresentation()
            {

            }
        }

        public class EmptySpace : Piece
        {
            public bool EPValid;
            public EmptySpace(string color, Position position) : base(color, position)
            {
                this.EPValid = false;
                this.AddConsoleRepresentation();
                this.type = "blank";
            }

            public override void AddConsoleRepresentation()
            {
                this.consoleRepresentation = ' ';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {

            }
        }

        public class Rook : Piece
        {
            public Rook(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "rook";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♜';
                else
                    this.consoleRepresentation = '♖';
            }


            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();

                this.ExploreNonDiagonals(board);
            }
        }

        public class Pawn : Piece
        {
            public Pawn(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "pawn";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♟';
                else
                    this.consoleRepresentation = '♙';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();

                this.CheckMoveForward(board);
                this.CheckDiscardPieceMove(board);
                this.CheckMoveForwardByTwo(board);
                this.CheckMoveEP(EPValidPosition, currentMove, lastEPUpdate);

            }

            private void CheckMoveForwardByTwo(Piece[,] board)
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

            private void CheckMoveEP(Position EPValidPosition, int currentMove, int lastEPUpdate)
            {

                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                if (currentMove - lastEPUpdate <= 1)
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


            private void CheckMoveForward(Piece[,] board)
            {
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                if (board[this.position.x + increment, this.position.y].color == "blank")
                    this.validMoves[this.position.x + increment, this.position.y] = true;
            }

            private void CheckDiscardPieceMove(Piece[,] board)
            {
                int increment;
                if (this.color == "black")
                    increment = 1;
                else
                    increment = -1;

                try
                {
                    if (IsOppositeColorsBW(board[this.position.x + increment, this.position.y + 1].color, this.color))
                        this.validMoves[this.position.x + increment, this.position.y + 1] = true;
                }
                catch
                {

                }

                try
                {
                    if (IsOppositeColorsBW(board[this.position.x + increment, this.position.y - 1].color, this.color))
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
                this.AddConsoleRepresentation();
                this.type = "knight";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♞';
                else
                    this.consoleRepresentation = '♘';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();
                int x = this.position.x;
                int y = this.position.y;
                for (int i = -2; i <= 2; i++)
                {
                    for (int j = -2; j <= 2; j++)
                    {
                        if (IsBetweenIncluding(x - i, 0, 7) && IsBetweenIncluding(y - j, 0, 7) && Math.Abs(i) + Math.Abs(j) == 3 && i != 0 && j != 0)
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
                this.AddConsoleRepresentation();
                this.type = "bishop";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♝';
                else
                    this.consoleRepresentation = '♗';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();

                this.ExploreDiagonals(board);


            }
        }

        public class Queen : Piece
        {
            public Queen(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "queen";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♛';
                else
                    this.consoleRepresentation = '♕';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();

                this.ExploreDiagonals(board);
                this.ExploreNonDiagonals(board);
            }
        }

        public class King : Piece
        {
            public King(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "king";
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♚';
                else
                    this.consoleRepresentation = '♔';
            }

            private bool CheckKingsNextMoveNextToKing(Piece[,] board, int rowIndex, int columnIndex)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (IsBetweenIncluding(rowIndex + i, 0, 7) && IsBetweenIncluding(columnIndex + j, 0, 7))
                        {
                            if (board[rowIndex + i, columnIndex + j].type == "king" && IsOppositeColorsBW(board[rowIndex + i, columnIndex + j].color, this.color))
                            {
                                return true;
                            }
                        }

                    }
                }
                return false;
            }

            private void CheckCastlePossibility(Piece[,] board, int rowIndex, int columnIndex)
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
                if (board[rowIndex, columnIndex - 1].type == "blank" && board[rowIndex, columnIndex - 2].type == "blank" && board[rowIndex, columnIndex - 3].type == "blank")
                {
                    if (board[rowIndex, columnIndex - 4].withoutMove == true)
                    {
                        this.validMoves[rowIndex, columnIndex - 2] = true;
                    }
                }

            }


            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (IsBetweenIncluding(this.position.x + i, 0, 7) && IsBetweenIncluding(this.position.y + j, 0, 7))
                        {
                            if (board[this.position.x + i, this.position.y + j].color != this.color)
                            {
                                if (!this.CheckKingsNextMoveNextToKing(board, this.position.x + i, this.position.y + j))
                                {
                                    this.validMoves[this.position.x + i, this.position.y + j] = true;
                                }
                            }
                        }
                    }
                }
                this.CheckCastlePossibility(board, this.position.x, this.position.y);

            }
        }



        public class Move
        {
            public string startPosition;
            public string endPosition;
            //flags
            public bool ep;
            public string castle;

            private readonly string[] columnIndexesToLetters = { "a", "b", "c", "d", "e", "f", "g", "h" };


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
                this.ep = false;
                this.castle = "none";
            }

            public Move(Position startPosition, Position endPosition)
            {
                this.startPosition = this.columnIndexesToLetters[startPosition.x] + ReverseNumber8((startPosition.y - 1)).ToString();
                this.endPosition = this.columnIndexesToLetters[endPosition.x] + ReverseNumber8((endPosition.y - 1)).ToString();
                this.ep = false;
                this.castle = "none";
            }

            private static int GetColumnIndex(string value)
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
                return asciiBytes[0] - 97;
            }

            public int GetColumnIndexStartPosition()
            {
                return GetColumnIndex(this.startPosition);
            }

            public int GetColumnIndexEndPosition()
            {
                return GetColumnIndex(this.endPosition);
            }

            public int GetRowIndexStartPosition()
            {
                int result = this.startPosition[1] - '0' - 1;
                return ReverseNumber8(result);
            }

            public int GetRowIndexEndPosition()
            {
                int result = this.endPosition[1] - '0' - 1;
                return ReverseNumber8(result);
            }

            public string GetStringRepresentation()
            {
                return this.startPosition + " " + this.endPosition;
            }
        }

        public class Communicator
        {
            private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            private const int PORT = 100;
            public string receivedMessage = "";
            public Communicator()
            {
                ConnectToServer();
                Thread thread = new(this.RequestLoop);
                thread.Start();
            }

            private static void ConnectToServer()
            {
                int attempts = 0;

                while (!ClientSocket.Connected)
                {
                    try
                    {
                        attempts++;
                        ClientSocket.Connect(IPAddress.Loopback, PORT);
                    }
                    catch (SocketException)
                    {
                    }
                }
            }
            private void RequestLoop()
            {                
                while (true)
                {
                    ReceiveResponse();
                }
            }

            public void SendResign()
            {
                this.SendString("00");
            }

            public void SendDrawOffer()
            {
                this.SendString("01");
            }



            public void SendString(string text)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }

            static public void ReceiveResponse()
            {
                var buffer = new byte[2048];
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0) return;
                var data = new byte[received];
                Array.Copy(buffer, data, received);
                string text = Encoding.ASCII.GetString(data);

                if (text == "white" || text == "black")
                {
                    playerColor = text;
                    MessageBox.Show("You play as " + text.ToUpper());
                    playerColor = text;
                    return;
                }
                
                infoForMoveInput = text;
                
            }
        }

        
    }
}