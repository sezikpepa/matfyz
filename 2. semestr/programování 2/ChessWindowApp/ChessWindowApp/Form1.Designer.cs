﻿namespace ChessWindowApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.opponentNameLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.offerDrawButton = new System.Windows.Forms.Button();
            this.resignButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.chooseOpponentComboBox = new System.Windows.Forms.ComboBox();
            this.showValidMovesCheckBox = new System.Windows.Forms.CheckBox();
            this.blackDiscardedPiecesPanel = new System.Windows.Forms.Panel();
            this.whiteDiscardedPiecesPanel = new System.Windows.Forms.Panel();
            this.ChoosePlayerColorCheckBox = new System.Windows.Forms.CheckBox();
            this.actualizationMoveFromServerTimer = new System.Windows.Forms.Timer(this.components);
            this.gameInfoLabel = new System.Windows.Forms.Label();
            this.EngineMoveButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // opponentNameLabel
            // 
            this.opponentNameLabel.AutoSize = true;
            this.opponentNameLabel.BackColor = System.Drawing.Color.DarkGray;
            this.opponentNameLabel.Font = new System.Drawing.Font("Monotype Corsiva", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.opponentNameLabel.Location = new System.Drawing.Point(956, 289);
            this.opponentNameLabel.Name = "opponentNameLabel";
            this.opponentNameLabel.Size = new System.Drawing.Size(81, 49);
            this.opponentNameLabel.TabIndex = 0;
            this.opponentNameLabel.Text = "You";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 800);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkGray;
            this.label2.Font = new System.Drawing.Font("Monotype Corsiva", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(1002, 765);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 49);
            this.label2.TabIndex = 2;
            this.label2.Text = "You";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(947, 75);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(227, 222);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(956, 540);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(184, 222);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // offerDrawButton
            // 
            this.offerDrawButton.BackColor = System.Drawing.Color.Orange;
            this.offerDrawButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.offerDrawButton.Location = new System.Drawing.Point(873, 407);
            this.offerDrawButton.Name = "offerDrawButton";
            this.offerDrawButton.Size = new System.Drawing.Size(118, 67);
            this.offerDrawButton.TabIndex = 0;
            this.offerDrawButton.Text = "1:1";
            this.offerDrawButton.UseVisualStyleBackColor = false;
            this.offerDrawButton.Click += new System.EventHandler(this.OfferDrawButtonClicked);
            // 
            // resignButton
            // 
            this.resignButton.BackColor = System.Drawing.Color.Red;
            this.resignButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.resignButton.Location = new System.Drawing.Point(1111, 407);
            this.resignButton.Name = "resignButton";
            this.resignButton.Size = new System.Drawing.Size(118, 67);
            this.resignButton.TabIndex = 8;
            this.resignButton.Text = "0:2";
            this.resignButton.UseVisualStyleBackColor = false;
            this.resignButton.Click += new System.EventHandler(this.ResignButtonClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(43, 852);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 50);
            this.label5.TabIndex = 9;
            this.label5.Text = "a";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(144, 855);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 50);
            this.label6.TabIndex = 10;
            this.label6.Text = "b";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(346, 855);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 50);
            this.label7.TabIndex = 12;
            this.label7.Text = "d";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(243, 855);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 50);
            this.label8.TabIndex = 11;
            this.label8.Text = "c";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(745, 855);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 50);
            this.label9.TabIndex = 16;
            this.label9.Text = "h";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(642, 855);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 50);
            this.label10.TabIndex = 15;
            this.label10.Text = "g";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(543, 855);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 50);
            this.label11.TabIndex = 14;
            this.label11.Text = "f";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(440, 855);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 50);
            this.label12.TabIndex = 13;
            this.label12.Text = "e";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(815, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 50);
            this.label13.TabIndex = 17;
            this.label13.Text = "8";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(818, 183);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 50);
            this.label14.TabIndex = 18;
            this.label14.Text = "7";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(821, 380);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 50);
            this.label15.TabIndex = 20;
            this.label15.Text = "5";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(818, 286);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(43, 50);
            this.label16.TabIndex = 19;
            this.label16.Text = "6";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label17.Location = new System.Drawing.Point(824, 588);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 50);
            this.label17.TabIndex = 22;
            this.label17.Text = "3";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label18.Location = new System.Drawing.Point(821, 480);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 50);
            this.label18.TabIndex = 21;
            this.label18.Text = "4";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label19.Location = new System.Drawing.Point(827, 788);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 50);
            this.label19.TabIndex = 24;
            this.label19.Text = "1";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label20.Location = new System.Drawing.Point(824, 680);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(43, 50);
            this.label20.TabIndex = 23;
            this.label20.Text = "2";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1124, 861);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 29);
            this.button4.TabIndex = 25;
            this.button4.Text = "RESET";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ResetButtonClicked);
            // 
            // chooseOpponentComboBox
            // 
            this.chooseOpponentComboBox.FormattingEnabled = true;
            this.chooseOpponentComboBox.Items.AddRange(new object[] {
            "Prvák informatik",
            "Bakalář",
            "Magistr",
            "Martin Pergel",
            "Náhodný člověk z internetu",
            "Jen tak si tahat"});
            this.chooseOpponentComboBox.Location = new System.Drawing.Point(947, 12);
            this.chooseOpponentComboBox.Name = "chooseOpponentComboBox";
            this.chooseOpponentComboBox.Size = new System.Drawing.Size(227, 28);
            this.chooseOpponentComboBox.TabIndex = 26;
            this.chooseOpponentComboBox.Text = "Jen tak si tahat";
            this.chooseOpponentComboBox.SelectedValueChanged += new System.EventHandler(this.ChooseOpponentComboBoxValueChanged);
            // 
            // showValidMovesCheckBox
            // 
            this.showValidMovesCheckBox.AutoSize = true;
            this.showValidMovesCheckBox.Location = new System.Drawing.Point(968, 866);
            this.showValidMovesCheckBox.Name = "showValidMovesCheckBox";
            this.showValidMovesCheckBox.Size = new System.Drawing.Size(150, 24);
            this.showValidMovesCheckBox.TabIndex = 27;
            this.showValidMovesCheckBox.Text = "Show valid moves";
            this.showValidMovesCheckBox.UseVisualStyleBackColor = true;
            this.showValidMovesCheckBox.CheckedChanged += new System.EventHandler(this.ShowValidMovesCheckBoxChanged);
            // 
            // blackDiscardedPiecesPanel
            // 
            this.blackDiscardedPiecesPanel.Location = new System.Drawing.Point(1182, 75);
            this.blackDiscardedPiecesPanel.Name = "blackDiscardedPiecesPanel";
            this.blackDiscardedPiecesPanel.Size = new System.Drawing.Size(261, 314);
            this.blackDiscardedPiecesPanel.TabIndex = 28;
            // 
            // whiteDiscardedPiecesPanel
            // 
            this.whiteDiscardedPiecesPanel.Location = new System.Drawing.Point(1182, 540);
            this.whiteDiscardedPiecesPanel.Name = "whiteDiscardedPiecesPanel";
            this.whiteDiscardedPiecesPanel.Size = new System.Drawing.Size(355, 321);
            this.whiteDiscardedPiecesPanel.TabIndex = 29;
            // 
            // ChoosePlayerColorCheckBox
            // 
            this.ChoosePlayerColorCheckBox.AutoSize = true;
            this.ChoosePlayerColorCheckBox.Location = new System.Drawing.Point(969, 892);
            this.ChoosePlayerColorCheckBox.Name = "ChoosePlayerColorCheckBox";
            this.ChoosePlayerColorCheckBox.Size = new System.Drawing.Size(247, 24);
            this.ChoosePlayerColorCheckBox.TabIndex = 30;
            this.ChoosePlayerColorCheckBox.Text = "Draw black pieces at the bottom";
            this.ChoosePlayerColorCheckBox.UseVisualStyleBackColor = true;
            this.ChoosePlayerColorCheckBox.CheckedChanged += new System.EventHandler(this.ChoosePlayerColorCheckBoxChanged);
            // 
            // actualizationMoveFromServerTimer
            // 
            this.actualizationMoveFromServerTimer.Interval = 200;
            this.actualizationMoveFromServerTimer.Tag = "internetTimer";
            this.actualizationMoveFromServerTimer.Tick += new System.EventHandler(this.CheckMoveFromInternet);
            // 
            // gameInfoLabel
            // 
            this.gameInfoLabel.AutoSize = true;
            this.gameInfoLabel.Location = new System.Drawing.Point(123, 16);
            this.gameInfoLabel.Name = "gameInfoLabel";
            this.gameInfoLabel.Size = new System.Drawing.Size(50, 20);
            this.gameInfoLabel.TabIndex = 31;
            this.gameInfoLabel.Text = "label1";
            this.gameInfoLabel.Visible = false;
            // 
            // EngineMoveButton
            // 
            this.EngineMoveButton.Location = new System.Drawing.Point(947, 46);
            this.EngineMoveButton.Name = "EngineMoveButton";
            this.EngineMoveButton.Size = new System.Drawing.Size(142, 29);
            this.EngineMoveButton.TabIndex = 32;
            this.EngineMoveButton.Text = "Do Engine Move";
            this.EngineMoveButton.UseVisualStyleBackColor = true;
            this.EngineMoveButton.Click += new System.EventHandler(this.button1_Clicks);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timerinput);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 659);
            this.Controls.Add(this.EngineMoveButton);
            this.Controls.Add(this.gameInfoLabel);
            this.Controls.Add(this.ChoosePlayerColorCheckBox);
            this.Controls.Add(this.whiteDiscardedPiecesPanel);
            this.Controls.Add(this.blackDiscardedPiecesPanel);
            this.Controls.Add(this.showValidMovesCheckBox);
            this.Controls.Add(this.chooseOpponentComboBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.resignButton);
            this.Controls.Add(this.offerDrawButton);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.opponentNameLabel);
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label opponentNameLabel;
        private Panel panel1;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button offerDrawButton;
        private Button resignButton;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Button button4;
        private ComboBox chooseOpponentComboBox;
        private CheckBox showValidMovesCheckBox;
        private Panel blackDiscardedPiecesPanel;
        private Panel whiteDiscardedPiecesPanel;
        private CheckBox ChoosePlayerColorCheckBox;
        public System.Windows.Forms.Timer actualizationMoveFromServerTimer;
        private Label gameInfoLabel;
        private Button EngineMoveButton;
        private System.Windows.Forms.Timer timer1;
    }
}