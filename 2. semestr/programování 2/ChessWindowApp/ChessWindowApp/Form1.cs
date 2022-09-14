﻿using System;
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

        private bool keepDisabled = false;
        private Button? lastClickedButton;

        public Position? startPosition;
        public Position? endPosition;

        public bool showValidMoves;

        public ChessEngine chessEngine = new();

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
            if(this.playAsWhite == false)
                this.RedrawBoardWhiteTop();

            this.SetDrawResignButtonDisability();
        }

        private void CheckMoveFromInternet(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = (System.Windows.Forms.Timer)sender;

            if (this.keepDisabled == true)
            {
                this.DisableChessGrid();
                return;
            }

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
                    this.gameInfoLabel.Text = "Opponent resign";
                    MessageBox.Show("Opponent resign");             
                    this.gameInfoLabel.Visible = true;

                    return;
                }

                else if (infoForMoveInput == "01")
                {
                    this.DisableChessGrid();

                    infoForMoveInput = "";
                    this.actualizationMoveFromServerTimer.Enabled = false;
                    this.actualizationMoveFromServerTimer.Stop();
                    this.actualizationMoveFromServerTimer = null;
                    stop = true;
                    timer.Tag = "stop";

                    if (this.gameInfoLabel.Text == "Opponent offered a draw") return;
                    this.gameInfoLabel.Text = "Opponent offered a draw";
                    this.gameInfoLabel.Visible = true;

                    DialogResult dialogResult = MessageBox.Show("Do you accept your opponent draw?", "Draw accept form", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.gameInfoLabel.Text = "DRAW";
                        this.gameInfoLabel.Visible = true;
                        this.DisableChessGrid();
                        this.onlineCommunicator.SendDrawAccepted();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.gameInfoLabel.Visible = true;
                        this.gameInfoLabel.Text = "You denied draw offer";
                        this.onlineCommunicator.SendDrawDenied();
                        if (playerColor != this.chessBoard.playerOnMove)
                        {
                            this.DisableChessGrid();
                        }
                        else
                        {
                            this.EnableChessGrid();
                        }
                    }
                    return;
                }

                else if (infoForMoveInput == "02")
                {
                    this.DisableChessGrid();

                    this.gameInfoLabel.Text = "DRAW accepted";
                    this.gameInfoLabel.Visible = true;
                    return;
                }

                else if (infoForMoveInput == "03")
                {
                    this.gameInfoLabel.Text = "DRAW offer denied, continue playing";
                    this.gameInfoLabel.Visible = true;

                    if (playerColor != this.chessBoard.playerOnMove)
                    {
                        this.DisableChessGrid();
                    }
                    else
                    {
                        this.EnableChessGrid();
                    }
                }

                else if (infoForMoveInput == "05")
                {
                    this.gameInfoLabel.Text = "Opponent has disconnected, you are winner";
                    this.gameInfoLabel.Visible = true;

                    this.DisableChessGrid();

                    infoForMoveInput = "";
                }

                else
                {
                    //MessageBox.Show(infoForMoveInput);
                    this.chessBoard.MoveInput(new Move(infoForMoveInput));
                    this.gameInfoLabel.Visible = false;
                    this.RedrawChessGrid();
                    if(this.playAsWhite == false)
                        this.RedrawBoardWhiteTop();
                    infoForMoveInput = "";

                    this.DisableChessGrid();
                    return;
                }
            }
            //if (this.gameInfoLabel.Visible == true) return;

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

        private void SetDrawResignButtonDisability()
        {
            if (this.mode == 0)
            {
                this.resignButton.Enabled = false;
                this.offerDrawButton.Enabled = false;
                return;
            }
            this.resignButton.Enabled = true;
            this.offerDrawButton.Enabled = true;

        }

        private void ResignButtonClicked(object sender, EventArgs e)
        {
            this.onlineCommunicator.SendResign();
            this.gameInfoLabel.Text = "You resign";
            this.gameInfoLabel.Visible = true;
            this.DisableChessGrid();
        }

        private void OfferDrawButtonClicked(object sender, EventArgs e)
        {
            this.onlineCommunicator.SendDrawOffer();
            this.gameInfoLabel.Text = "You offered a draw, waiting for opponent reaction";
            this.gameInfoLabel.Visible = true;
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
                    //brnGrid[i, j].Text = Char.ToString(chessEngine.currentChessBoard.board[j, i].consoleRepresentation);
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
            this.chooseOpponentComboBox.Enabled = false;

            if (sender != null)
            {
                if (this.lastClickedButton != null)
                    this.SetButtonOriginalColor(this.lastClickedButton);

                Button clickedButton = (Button)sender;
                Point location = (Point)clickedButton.Tag;
                if(this.playAsWhite == false)
                {
                    location.Y = ReverseNumber8(location.Y);
                }

                if (!this.startPosition.IsValid())
                {
                    if(this.chessBoard.board[location.Y, location.X].color == this.chessBoard.playerOnMove)
                    {
                        this.startPosition = new Position(location.X, location.Y);
                        clickedButton.BackColor = Color.LightBlue;
                        this.chessBoard.board[location.Y, location.X].GenerateValidMoves(this.chessBoard.board, this.chessBoard.positionEPValid, this.chessBoard.currentMove, this.chessBoard.lastEPUpdate);
                        //this.chessBoard.clearFromValidMovesCheckedSquares();
                        if (this.showValidMoves)
                        {
                            //this.chessBoard.clearFromValidMovesCheckedSquare(location.X, location.Y);
                            
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
                if(this.playAsWhite == false)
                    this.RedrawBoardWhiteTop(); //pozorddddddddddddddddddddddddddddddddddddddddddddddddddddd
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
                        if (this.playAsWhite == false)
                        {                           
                            int y = ReverseNumber8(i);
                            if (this.chessBoard.kingInCheckAfterMove(this.startPosition.y, this.startPosition.x, i, j, this.chessBoard.board[this.startPosition.y, this.startPosition.x].color) == true)
                                continue;
                            if (this.chessBoard.board[i, j].type == "blank")
                            {
                                this.brnGrid[j, y].BackColor = Color.LightGreen;
                            }
                            else
                            {
                                this.brnGrid[j, y].BackColor = Color.Red;
                            }
                        }
                        else
                        {
                            //MessageBox.Show(this.startPosition.x.ToString() + this.startPosition.y.ToString() + i.ToString()+  j.ToString() + this.chessBoard.board[this.startPosition.y, this.startPosition.x].color + this.chessBoard.board[this.startPosition.y, this.startPosition.x].type);
                            //if (this.chessBoard.board[this.startPosition.y, this.startPosition.y].color == "blank")
                            //MessageBox.Show(i.ToString()+ j.ToString());
                            //MessageBox.Show(this.chessBoard.board[this.startPosition.y, this.startPosition.x].type);
                            if (this.chessBoard.kingInCheckAfterMove(this.startPosition.y, this.startPosition.x, i, j, this.chessBoard.board[this.startPosition.y, this.startPosition.x].color) == true)
                                continue;
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
        }

        private void GuiMoveInput(Position startPosition, Position endPosition)
        {
            Move move = new(startPosition, endPosition);

            var result = chessBoard.MoveInput(move);
            this.RedrawChessGrid();
            chessEngine.currentChessBoard.MoveInput(move);
            if (result == true)
            {
                this.button1_Click();
            }

            if (this.mode == 1)
                this.onlineCommunicator.SendString(move.GetStringRepresentation());

            this.RedrawChessGrid();


        }

        private void ChoosePlayerColorCheckBoxChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            this.SetAllButtonOriginalColor();

            if (checkBox.Checked)
            {
                this.playAsWhite = false;
                this.RedrawBoardWhiteTop();
            }
            else
            {
                this.playAsWhite = true;
                this.RedrawChessGrid();
            }         
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
            }
            if (choosePlayerComboBox.Text == "Jen tak si tahat")
            {
                this.mode = 0;
                this.actualizationMoveFromServerTimer.Stop();
            }
            if (choosePlayerComboBox.Text == "Prvák informatik")
            {
                this.mode = 2;
                ChessBoard naengine = new ChessBoard();
                this.chessEngine = new ChessEngine(naengine, 0);
                this.actualizationMoveFromServerTimer.Stop();
            }
            if (choosePlayerComboBox.Text == "Bakalář")
            {
                this.mode = 2;
                ChessBoard naengine = new ChessBoard();
                this.chessEngine = new ChessEngine(naengine, 1);
                this.actualizationMoveFromServerTimer.Stop();
            }
            if (choosePlayerComboBox.Text == "Magistr")
            {
                this.mode = 2;
                ChessBoard naengine = new ChessBoard();
                this.chessEngine = new ChessEngine(naengine, 2);
                this.actualizationMoveFromServerTimer.Stop();
            }
            if (choosePlayerComboBox.Text == "Martin Pergel")
            {
                this.mode = 2;
                ChessBoard naengine = new ChessBoard();
                this.chessEngine = new ChessEngine(naengine, 3);
                this.actualizationMoveFromServerTimer.Stop();
            }

            this.SetDrawResignButtonDisability();

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
            if (this.playAsWhite == false)
            {
                this.RedrawBoardWhiteTop();
            }
            this.ResetPositions();

            this.chessBoard.ResetDiscardedPieces();

            this.SetAllButtonOriginalColor();
            this.SetValuesDiscardePiecesLabels();

            this.EnableChessGrid();

            this.keepDisabled = false;
            this.chooseOpponentComboBox.Enabled = true;

            this.gameInfoLabel.Visible = false;
            this.gameInfoLabel.Text = "";

            this.chessEngine.currentChessBoard = new ChessBoard();

            if(this.mode == 1)
            {
                this.onlineCommunicator.SendReset();
                this.onlineCommunicator = new();
                infoForMoveInput = "";
            }

        }

        static public int ReverseNumber8(int value)
        {
            if (value < 0 || value > 7)
                return 0;

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

            public void NormalizeWhiteTop()
            {
                this.y = ReverseNumber8(this.y);
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

            public string id;
            public float potencionalValueByEngine;

            public bool lastMoveCorrect = true;

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

                this.id = "";

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
            
            /*
            public void SetStartPosition()
            {
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        this.board[j, i] = new EmptySpace("blank", new Position(j, i));
                    }
                }

                this.board[7, 6] = new Queen("white", new Position(7, 6));
                this.board[7, 7] = new Queen("white", new Position(7, 7));
                this.board[0, 7] = new Queen("black", new Position(0, 7));
                this.board[1, 7] = new Pawn("white", new Position(1, 7));
                this.board[7, 0] = new Knight("black", new Position(7, 0));

            }
            */
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
                    return;
                }
                this.playerOnMove = "white";
                
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

            public bool MoveInput(Move move)
            {
               
                int rowStart = move.GetRowIndexStartPosition();
                int rowEnd = move.GetRowIndexEndPosition();
                int columnStart = move.GetColumnIndexStartPosition();
                int columnEnd = move.GetColumnIndexEndPosition();

                //MessageBox.Show(this.board[rowStart, columnStart].type);

                if (!this.CheckMoveFromRightPlayer(rowStart, columnStart))
                {
                    this.lastMoveCorrect = false;
                    return false;
                }

                if (!this.CheckCorrectPieceMove(rowStart, columnStart, rowEnd, columnEnd))
                {
                    this.lastMoveCorrect = false;
                    return false;
                }
                //MessageBox.Show("přes kontrolu");
                if (this.kingInCheckAfterMove(rowStart, columnStart, rowEnd, columnEnd, this.playerOnMove))
                {
                    //MessageBox.Show("check condition");
                    return false;
                }
                    

                this.CheckForMarkingEPValidSquare(rowStart, columnStart, rowEnd, columnEnd);

                if (this.board[rowStart, columnStart].type == "king")
                {
                    Console.WriteLine("king");
                    if (columnEnd - columnStart == 2)
                    {
                        this.DoShortCastle(this.board[rowStart, columnStart].color);
                        this.lastMoveCorrect=true;
                        return true;
                    }
                    else if (columnEnd - columnStart == -2)
                    {
                        this.DoLongCastle(this.board[rowStart, columnStart].color);
                        this.lastMoveCorrect = true;
                        return true;
                    }
                }

                this.MakeMove(rowStart, columnStart, rowEnd, columnEnd);
                
                this.lastMoveCorrect = true;

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
                return true;
            }

            private void MakeMove(int rowStart, int columnStart, int rowEnd, int columnEnd)
            {
                //MessageBox.Show("tah proveden");
                if (this.board[rowEnd, columnEnd].type != "blank")
                {
                    this.ChangeDiscardedPiecesCount(this.board[rowEnd, columnEnd]);
                }
                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart];
                this.ClearSquare(rowStart, columnStart);
                this.PromotePawns();
                this.IncreaseCurrentMove();
            }

            private void PromotePawns()
            {
                for(int i = 0; i < 8; i++)
                {
                    if(this.board[0, i].type == "pawn")
                    {
                        this.board[0, i] = new Queen("white" , new Position(0, i));
                    }
                    if(this.board[7, i].type == "pawn")
                    {
                        this.board[7, i] = new Queen("black" , new Position(7, i));
                    }
                }
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

            public bool kingInCheckAfterMove(int rowStart, int columnStart, int rowEnd, int columnEnd, string color)
            {
                Piece start = this.board[rowStart, columnStart];
                Piece end = this.board[rowEnd, columnEnd];

                this.board[rowEnd, columnEnd] = this.board[rowStart, columnStart]; //tady bylo copy, kdyby nefungoval správně šach
                this.board[rowStart, columnStart] = new EmptySpace("blank", new Position(rowStart, columnStart));

                bool result = this.IsKingChecked(this.board, color);

                this.board[rowStart, columnStart] = start;
                this.board[rowEnd, columnEnd] = end;

                return result;
            }

            private Position FindKing(Piece[,] board, string color)
            {
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        if(board[i, j].type == "king")
                        {
                            if(board[i, j].color == color)
                            {
                                return new Position(i, j);
                            }
                        }
                    }
                }
                return new Position(-1, -1);
            }

            public bool IsKingChecked(Piece[,] board, string color)
            {
                Position kingPosition = this.FindKing(board, color); 
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[i, j].GenerateValidMoves(board, this.positionEPValid, this.currentMove, this.lastEPUpdate);
                        if (board[i, j].validMoves[kingPosition.x, kingPosition.y] == true)
                        {
                            return true;
                        }
                    }
                }
                return false;
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

            public ChessBoard Copy()
            {
                ChessBoard returnChessboard = new();
                
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        if(this.board[i, j].type == "rook")
                        {
                            returnChessboard.board[i, j] = new Rook(this.board[i, j].color, new Position(i, j));
                        }
                        else if (this.board[i, j].type == "knight")
                        {
                            returnChessboard.board[i, j] = new Knight(this.board[i, j].color, new Position(i, j));
                        }
                        else if (this.board[i, j].type == "bishop")
                        {
                            returnChessboard.board[i, j] = new Bishop(this.board[i, j].color, new Position(i, j));
                        }
                        else if (this.board[i, j].type == "king")
                        {
                            returnChessboard.board[i, j] = new King(this.board[i, j].color, new Position(i, j));
                        }
                        else if (this.board[i, j].type == "queen")
                        {
                            returnChessboard.board[i, j] = new Queen(this.board[i, j].color, new Position(i, j));
                        }
                        else if (this.board[i, j].type == "pawn")
                        {
                            returnChessboard.board[i, j] = new Pawn(this.board[i, j].color, new Position(i, j));
                        }
                        else
                        {
                            returnChessboard.board[i, j] = new EmptySpace(this.board[i, j].color, new Position(i, j));
                        }
                        for(int k = 0; k < 8; k++)
                        {
                            for(int l = 0; l < 8; l++)
                            {
                                returnChessboard.board[i, j].validMoves[k, l] = this.board[i, j].validMoves[k, l];
                            }
                        }
                    }
                }



                returnChessboard.positionEPValid = this.positionEPValid;
                returnChessboard.playerOnMove = this.playerOnMove;
                returnChessboard.currentMove = this.currentMove;
                returnChessboard.lastEPUpdate = this.lastEPUpdate;
                returnChessboard.potencionalValueByEngine = this.potencionalValueByEngine;
                
                


                return returnChessboard;
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
            public int[,] strongPositions;

            public Piece(string color, Position position)
            {
                this.CheckValidityColorInput(color);
                this.consoleRepresentation = ' ';
                this.validMoves = new bool[8, 8];
                this.position = position;
                this.withoutMove = true;
                this.type = "";
                this.color = color;
                /*
                this.strongPositions = new int[8,8] { 
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    { 0,0,0,0,0,0,0,0 },
                                                    };*/
            }

            public int GetPositionStrongValue()
            {
                if (this.color == "white")
                {
                    //MessageBox.Show(this.strongPositions[this.position.x, this.position.y].ToString() + this.position.x.ToString() + this.position.y.ToString());
                    //MessageBox.Show(this.strongPositions[this.position.x, this.position.y].ToString());
                    return this.strongPositions[this.position.x, this.position.y];
                }
                    
                else if (this.color == "black")
                {                  
                    return this.strongPositions[ReverseNumber8(this.position.x), this.position.y];
                }
                    
                else
                    return 125;

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
                this.position.x = position.x;
                this.position.y = position.y;
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

            public virtual Piece Copy()
            {
                Piece piece = new(this.color, this.position);
                return piece;
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

            public override EmptySpace Copy()
            {
                EmptySpace piece = new EmptySpace(this.color, this.position);
                piece.withoutMove = false;
                piece.type = "blank";
                piece.color = this.color;
                return piece;

            }

            public override void AddConsoleRepresentation()
            {
                this.consoleRepresentation = ' ';
            }

            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();
            }
        }

        public class Rook : Piece
        {
            public Rook(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "rook";
                this.strongPositions = new int[8, 8] {
                                                    {  0,  0,  0,  0,  0,  0,  0,  0 },
                                                    {  5, 10, 10, 10, 10, 10, 10,  5 },
                                                    { -5,  0,  0,  0,  0,  0,  0, -5 },
                                                    { -5,  0,  0,  0,  0,  0,  0, -5 },
                                                    { -5,  0,  0,  0,  0,  0,  0, -5 },
                                                    { -5,  0,  0,  0,  0,  0,  0, -5 },
                                                    { -5,  0,  0,  0,  0,  0,  0, -5 },
                                                    {  0,  0,  0,  5,  5,  0,  0,  0 }
                                                    };
            }

            public override Rook Copy()
            {
                Rook piece = new Rook(this.color, new Position(this.position.x, this.position.y));
                piece.type = "rook";
                piece.color = this.color;
                piece.withoutMove = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }
                return piece;

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
                this.strongPositions = new int[8, 8] {
                                                    {  1600,  1600,  1600,  1600,  1600,  1600,  1600,  1600 },
                                                    { 50, 50, 50, 50, 50, 50, 50, 50 },
                                                    { 10, 10, 20, 30, 30, 20, 10, 10 },
                                                    {  5,  5, 10, 25, 25, 10,  5,  5 },
                                                    {  0,  0,  0, 20, 20,  0,  0,  0 },
                                                    {  5, -5,-10,  0,  0,-10, -5,  5 },
                                                    {  5, 10, 10,-20,-20, 10, 10,  5 },
                                                    {  0,  0,  0,  0,  0,  0,  0,  0 }
                                                    };
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

            public override Pawn Copy()
            {
                Pawn piece = new Pawn(this.color, new Position(this.position.x, this.position.y));
                piece.withoutMove = false;
                piece.type = "pawn";
                piece.color = this.color;

                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }

                return piece;

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

                try
                {
                    if (board[this.position.x + increment, this.position.y].color == "blank")
                    {
                        if (board[this.position.x + (increment * 2), this.position.y].color == "blank")
                        {
                            if ((this.position.x == 1 && this.color == "black") || (this.position.x == 6 && this.color == "white"))
                                this.validMoves[this.position.x + (increment * 2), this.position.y] = true;
                        }
                    }
                }
                catch { }
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

                try
                {
                    if (board[this.position.x + increment, this.position.y].color == "blank")
                        this.validMoves[this.position.x + increment, this.position.y] = true;
                }
                catch
                {

                }
                
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
                this.strongPositions = new int[8, 8] {
                                                     { -50, -40, -30, -30, -30, -30, -40, -50 },
                                                    { -40, -20,   0,   0,   0,   0, -20, -40 },
                                                    { -30,   0,  10,  15,  15,  10,   0, -30 },
                                                    { -30,   5,  15,  20,  20,  15,   5, -30 },
                                                    { -30,   0,  15,  20,  20,  15,   0, -30 },
                                                    { -30,   5,  10,  15,  15,  10,   5, -30 },
                                                    { -40, -20,   0,   5,   5,   0, -20, -40 },
                                                    { -50, -40, -30, -30, -30, -30, -40, -50 }
                                                    };
            }

            public override void AddConsoleRepresentation()
            {
                if (this.color == "black")
                    this.consoleRepresentation = '♞';
                else
                    this.consoleRepresentation = '♘';
            }

            public override Knight Copy()
            {
                Knight piece = new Knight(this.color, new Position(this.position.x, this.position.y));
                piece.withoutMove = false;
                piece.type = "knight";
                piece.color = this.color;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }
                return piece;

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
                this.strongPositions = new int[8, 8] {
                                                      { -20,-10,-10,-10,-10,-10,-10,-20 },
                                                        { -10,  0,  0,  0,  0,  0,  0,-10 },
                                                        { -10,  0,  5, 10, 10,  5,  0,-10 },
                                                        { -10,  5,  5, 10, 10,  5,  5,-10 },
                                                        { -10,  0, 10, 10, 10, 10,  0,-10 },
                                                        { -10, 10, 10, 10, 10, 10, 10,-10 },
                                                        { -10,  5,  0,  0,  0,  0,  5,-10 },
                                                        { -20,-10,-10,-10,-10,-10,-10,-20 }
                                                    };
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

            public override Bishop Copy()
            {
                Bishop piece = new Bishop(this.color, new Position(this.position.x, this.position.y));
                piece.withoutMove = false;
                piece.type = "bishop";
                piece.color = this.color;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }
                return piece;

            }
        }

        public class Queen : Piece
        {
            public Queen(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation();
                this.type = "queen";
                this.strongPositions = new int[8, 8] {
                                                      { -20,-10,-10, -5, -5,-10,-10,-20 },
                                                        { -10,  0,  0,  0,  0,  0,  0,-10 },
                                                        { -10,  0,  5,  5,  5,  5,  0,-10 },
                                                        {  -5,  0,  5,  5,  5,  5,  0, -5 },
                                                        {   0,  0,  5,  5,  5,  5,  0, -5 },
                                                        { -10,  5,  5,  5,  5,  5,  0,-10 },
                                                        { -10,  0,  5,  0,  0,  0,  0,-10 },
                                                        { -20,-10,-10, -5, -5,-10,-10,-20 } 
                };
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

            public override Queen Copy()
            {
                Queen piece = new Queen(this.color, new Position(this.position.x, this.position.y));
                piece.withoutMove = false;
                piece.type = "queen";
                piece.color = this.color;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }
                return piece;

            }
        }

        public class King : Piece
        {
            public King(string color, Position position) : base(color, position)
            {
                this.AddConsoleRepresentation(); 
                this.type = "king";
                this.strongPositions = new int[8, 8] {
                                                      { -30,-40,-40,-50,-50,-40,-40,-30 },
                                                        { -30,-40,-40,-50,-50,-40,-40,-30 },
                                                        { -30,-40,-40,-50,-50,-40,-40,-30 },
                                                        { -30,-40,-40,-50,-50,-40,-40,-30 },
                                                        { -20,-30,-30,-40,-40,-30,-30,-20 },
                                                        { -10,-20,-20,-20,-20,-20,-20,-10 },
                                                        {  20, 20,  0,  0,  0,  0, 20, 20 },
                                                        {  20, 30, 10,  0,  0, 10, 30, 20 }
                                                    };
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
                try
                {
                    if (board[rowIndex, columnIndex + 1].type == "blank" && board[rowIndex, columnIndex + 2].type == "blank")
                    {
                        if (board[rowIndex, columnIndex + 3].withoutMove == true && board[rowIndex, columnIndex + 3].type == "rook" && board[rowIndex, columnIndex + 3].color == this.color)
                        {
                            this.validMoves[rowIndex, columnIndex + 2] = true;
                        }
                    }
                }
                catch
                {

                }

                //long castle
                try
                {

                    if (board[rowIndex, columnIndex - 1].type == "blank" && board[rowIndex, columnIndex - 2].type == "blank" && board[rowIndex, columnIndex - 3].type == "blank")
                    {
                        if (board[rowIndex, columnIndex - 4].withoutMove == true && board[rowIndex, columnIndex - 4].type == "rook" && board[rowIndex, columnIndex - 4].type == "rook")
                        {
                            this.validMoves[rowIndex, columnIndex - 2] = true;
                        }
                    }
                }
                catch
                {

                }
            }

            public override King Copy()
            {
                King piece = new King(this.color, new Position(this.position.x, this.position.y));
                piece.withoutMove = false;
                piece.type = "king";
                piece.color = this.color;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        piece.validMoves[i, j] = this.validMoves[i, j];
                    }
                }
                return piece;

            }


            public override void GenerateValidMoves(Piece[,] board, Position EPValidPosition, int currentMove, int lastEPUpdate)
            {
                this.ResetValidMoves();
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
            public string pieceToPromote;
            public float value;

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
                this.pieceToPromote = "";

            }

            public Move(string startPosition, string endPostion, string pieceToPromote="")
            {
                this.startPosition = startPosition;
                this.endPosition = endPostion;
                this.ep = false;
                this.castle = "none";
                this.pieceToPromote = pieceToPromote;
            }

            public Move(Position startPosition, Position endPosition, string pieceToPromote="")
            {
                this.startPosition = this.columnIndexesToLetters[startPosition.x] + ReverseNumber8((startPosition.y - 1)).ToString();
                this.endPosition = this.columnIndexesToLetters[endPosition.x] + ReverseNumber8((endPosition.y - 1)).ToString();
                this.ep = false;
                this.castle = "none";
                this.pieceToPromote = pieceToPromote;
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
            private static readonly Socket ClientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

            public void SendDrawAccepted()
            {
                this.SendString("02");
            }

            public void SendDrawDenied()
            {
                this.SendString("03");
            }

            public void SendStillConnectedStatus()
            {
                this.SendString("04");
            }

            public void SendReset()
            {
                this.SendString("05");
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
                    return;
                }
                
                infoForMoveInput = text;
                
            }
        }

        public class ChessEngine
        {
            public ChessBoard currentChessBoard;
            private int[] chessPiecesValues = { 1, 3, 3, 5, 8, 0 };
            private Dictionary<string, int> chessPiecesToIndex;
            public int remainingDeep;

            private Queue<ChessBoard> chessBoardsToEvaluate;
            private List<ChessBoard> evaluatedChessBoards;
            public ChessEngine(ChessBoard chessBoard=null, int remainingDeep=2)
            {
                if(chessBoard == null)
                    this.currentChessBoard = new ChessBoard();
                else
                {
                    this.currentChessBoard = chessBoard.Copy();
                }
                this.chessPiecesToIndex = new Dictionary<string, int>
                {
                    { "pawn", 0 },
                    { "knight", 1 },
                    { "bishop", 2 },
                    { "rook", 3 },
                    { "queen", 4 },
                    { "king", 5 },
                    { "blank", 5 }
                };

                this.chessBoardsToEvaluate = new Queue<ChessBoard>();
                this.evaluatedChessBoards = new List<ChessBoard>();
                this.remainingDeep = remainingDeep;

            }

            public void startExploring()
            {
                //Thread thread = new(this.GenerateBoards);
                //thread.Start();
                this.GenerateBoards();
                this.EvaluateBoards();
            }

            private void GenerateBoards()
            {
                ChessBoard chessBoard = this.currentChessBoard;
                chessBoard.GenerateMovesAllPieces();
                for (int i = 0; i < 8; i++) 
                {
                    for(int j = 0; j < 8; j++)
                    {                     
                        for(int k = 0; k < 8; k++)
                        {
                            for(int l = 0; l < 8; l++)
                            {              
                                if(chessBoard.board[i, j].color == chessBoard.playerOnMove)
                                {
                                    if (chessBoard.board[i, j].validMoves[k, l] == true)
                                    {
                                        ChessBoard newChessBoard = chessBoard.Copy();

                                        if (newChessBoard.kingInCheckAfterMove(i, j, k, l, chessBoard.playerOnMove))
                                        {
                                            continue;
                                        }
                                        
                                        

                                        newChessBoard.board[k, l] = newChessBoard.board[i, j];
                                        newChessBoard.board[i, j] = new EmptySpace("blank", new Position(i, j));

                                        newChessBoard.id = i.ToString() + " " + j.ToString() + " " + k.ToString() + " " + l.ToString();

                                        newChessBoard.board[k, l].UpdateCurrentPosition(new Position(k, l));
                                        //newChessBoard.board[i, j].UpdateCurrentPosition(new Position(i, j));      zbytečnost

                                        this.chessBoardsToEvaluate.Enqueue(newChessBoard);

                                    }
                                }
                                
                            }
                        }
                    }
                }
            }

            public Move getBestValue(string playerOnMove)
            {
                this.startExploring();



                float maximum = -1000;
                string id = "";
                if(playerOnMove == "white")
                {
                    maximum = -1000;
                    foreach (var element in this.evaluatedChessBoards)
                    {
                        //MessageBox.Show(element.id + " " + element.potencionalValueByEngine);
                        if(this.remainingDeep > 0)
                        {
                            ChessEngine newEngine = new ChessEngine(element, this.remainingDeep - 1);
                            newEngine.currentChessBoard.playerOnMove = "black";
                          
                            element.potencionalValueByEngine = newEngine.getBestValue("black").value;
                        }
                        if (element.potencionalValueByEngine > maximum)
                        {
                            maximum = element.potencionalValueByEngine;                           
                            id = element.id;
                        }

                    }
                }
                else// if (playerOnMove == "black")
                {
                    maximum = 1000;
                    foreach (var element in this.evaluatedChessBoards)
                    {
                        if (this.remainingDeep > 0)
                        {
                            ChessEngine newEngine = new ChessEngine(element, this.remainingDeep - 1);
                            newEngine.currentChessBoard.playerOnMove = "white";                          
                            element.potencionalValueByEngine = newEngine.getBestValue("white").value;
                        }
                        if (element.potencionalValueByEngine < maximum)
                        {
                            maximum = element.potencionalValueByEngine;
                            id = element.id;
                        }

                    }
                }

                this.evaluatedChessBoards.Clear();

                string[] parts = id.Split(' ');
                try
                {
                    Move returnMove = new Move(new Position(Int32.Parse(parts[1]), Int32.Parse(parts[0])), new Position(Int32.Parse(parts[3]), Int32.Parse(parts[2])));
                    returnMove.value = maximum;
                    return returnMove;
                }
                catch
                {
                    Move returnMove = new Move("e0", "d0");
                    if (this.currentChessBoard.IsKingChecked(this.currentChessBoard.board, this.currentChessBoard.playerOnMove))
                    {
                        if(playerOnMove == "black")
                        {
                            returnMove.value = -10000;
                        }
                        else
                        {
                            returnMove.value = 10000;
                        }
                        return returnMove;
                    }
                    MessageBox.Show("mat");

                    returnMove.value = 0;
                    return returnMove;

                    
                }               
            }

            public void EvaluateBoards()
            {
                while (this.chessBoardsToEvaluate.Count > 0)
                {
                    ChessBoard chessBoard = this.chessBoardsToEvaluate.Dequeue();
                    float value = 0;
                    if(this.remainingDeep == 0)
                        value = this.EvaluatePosition(chessBoard.board);

                    chessBoard.potencionalValueByEngine = value;
                    this.evaluatedChessBoards.Add(chessBoard);
                }
            }

            private float EvaluatePosition(Piece[,] board)
            {
                float result = 0;

                result += this.EvaluatePositionPieceValue(board);
                result += this.EvaluatePositionPiecePosition(board);
                result += this.DoublePawnPenalty(board);

                return result;
                
            }

            public float EvaluatePositionPieceValue(Piece[,] board)
            {
                int whiteScore = 0;
                int blackScore = 0;

                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        if(board[i, j].color == "white")
                        {
                            whiteScore += this.chessPiecesValues[this.chessPiecesToIndex[board[i, j].type]];
                        }
                        else if (board[i, j].color == "black")
                        {
                            blackScore += this.chessPiecesValues[this.chessPiecesToIndex[board[i, j].type]];
                        }
                    }
                }
                return whiteScore - blackScore;
            }

            public float EvaluatePositionPiecePosition(Piece[,] board)
            {
                int whiteScore = 0;
                int blackScore = 0;

                for(int i=0; i<8; i++)
                {
                    for (int j=0; j<8; j++)
                    {
                        if(board[i, j].color == "white")
                        {
                            whiteScore += board[i, j].GetPositionStrongValue();
                        }
                        else if(board[i, j].color == "black")
                        {
                            blackScore += board[i, j].GetPositionStrongValue();

                        }
                    }
                }
                return (float)((whiteScore - blackScore) / 160.0);
            }

            public float DoublePawnPenalty(Piece[,] board)
            {
                float whiteScore = 0;
                float blackScore = 0;

                int pawnCounterWhite;
                int pawnCounterBlack;

                for(int y = 0; y < 8; y++)
                {
                    pawnCounterWhite = 0;
                    pawnCounterBlack = 0;
                    for(int x = 0; x < 8; x++)
                    {
                        if(board[x, y].type == "pawn")
                        {
                            if(board[x, y].color == "white")
                            {
                                pawnCounterWhite++;
                            }
                            else if(board[x, y].color == "black")
                            {
                                pawnCounterBlack++;
                            }
                        }
                    }
                    whiteScore += (float)((pawnCounterWhite - 1) * 0.15);
                    blackScore += (float)((pawnCounterBlack - 1) * 0.15);
                }

                return whiteScore - blackScore;
            }

            public float SinglePawnPenalty(Piece[,] board)
            {
                float whiteScore = 0;
                float blackScore = 0;
                return 0;

            }


        }

        private void button1_Click()
        {
            //Button button = (Button) sender;
            if (this.mode != 2)
                return;
            var timeStart = DateTime.Now;
            Move engineMove = this.chessEngine.getBestValue(this.chessEngine.currentChessBoard.playerOnMove);
            var timeEnd = DateTime.Now;
            //MessageBox.Show((timeEnd - timeStart).TotalSeconds.ToString());
            //MessageBox.Show(engineMove.value.ToString());
            /*
            if (engineMove.GetStringRepresentation() == "e0 d0")
            {
                this.DisableChessGrid();
                MessageBox.Show("mat");
            }
            */   

            this.chessBoard.MoveInput(engineMove);
            this.chessEngine.currentChessBoard.MoveInput(engineMove);

            this.chessBoard.GenerateMovesAllPieces();
            this.chessEngine.currentChessBoard.GenerateMovesAllPieces();

            if (this.playAsWhite == false)
                this.RedrawBoardWhiteTop();
            else
                this.RedrawChessGrid();
            this.PrintDiscardedPieces();

        }

        private void EngineMoveButton_Click(object sender, EventArgs e)
        {
            //this.button1_Clicks();
        }

        private void button1_Clicks(object sender, EventArgs e)
        {
            MessageBox.Show("Enginy začínají hrát");
            this.button1_Click();
            this.timer1.Enabled = true;
            this.timer1.Start();
        }

        private void timerinput(object sender, EventArgs e)
        {
            this.button1_Click();
        }
    }
}