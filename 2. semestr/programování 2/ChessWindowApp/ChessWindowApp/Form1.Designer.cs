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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.button1 = new System.Windows.Forms.Button();
            this.sendConnectedStatusTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // opponentNameLabel
            // 
            this.opponentNameLabel.AutoSize = true;
            this.opponentNameLabel.BackColor = System.Drawing.Color.DarkGray;
            this.opponentNameLabel.Font = new System.Drawing.Font("Monotype Corsiva", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.opponentNameLabel.Location = new System.Drawing.Point(1554, 462);
            this.opponentNameLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.opponentNameLabel.Name = "opponentNameLabel";
            this.opponentNameLabel.Size = new System.Drawing.Size(128, 79);
            this.opponentNameLabel.TabIndex = 0;
            this.opponentNameLabel.Text = "You";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(20, 88);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 1280);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkGray;
            this.label2.Font = new System.Drawing.Font("Monotype Corsiva", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(1628, 1224);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 79);
            this.label2.TabIndex = 2;
            this.label2.Text = "You";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1539, 120);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(369, 355);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1554, 864);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(299, 355);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(1628, 549);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 75);
            this.label3.TabIndex = 5;
            this.label3.Text = "00:00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(1628, 768);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 75);
            this.label4.TabIndex = 6;
            this.label4.Text = "00:00";
            // 
            // offerDrawButton
            // 
            this.offerDrawButton.BackColor = System.Drawing.Color.Orange;
            this.offerDrawButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.offerDrawButton.Location = new System.Drawing.Point(1419, 651);
            this.offerDrawButton.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.offerDrawButton.Name = "offerDrawButton";
            this.offerDrawButton.Size = new System.Drawing.Size(192, 107);
            this.offerDrawButton.TabIndex = 0;
            this.offerDrawButton.Text = "1:1";
            this.offerDrawButton.UseVisualStyleBackColor = false;
            this.offerDrawButton.Click += new System.EventHandler(this.offerDrawButtonClicked);
            // 
            // resignButton
            // 
            this.resignButton.BackColor = System.Drawing.Color.Red;
            this.resignButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.resignButton.Location = new System.Drawing.Point(1805, 651);
            this.resignButton.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.resignButton.Name = "resignButton";
            this.resignButton.Size = new System.Drawing.Size(192, 107);
            this.resignButton.TabIndex = 8;
            this.resignButton.Text = "0:2";
            this.resignButton.UseVisualStyleBackColor = false;
            this.resignButton.Click += new System.EventHandler(this.ResignButtonClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(70, 1363);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 81);
            this.label5.TabIndex = 9;
            this.label5.Text = "a";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(234, 1368);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 81);
            this.label6.TabIndex = 10;
            this.label6.Text = "b";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(562, 1368);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 81);
            this.label7.TabIndex = 12;
            this.label7.Text = "d";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(395, 1368);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 81);
            this.label8.TabIndex = 11;
            this.label8.Text = "c";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(1211, 1368);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 81);
            this.label9.TabIndex = 16;
            this.label9.Text = "h";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(1043, 1368);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 81);
            this.label10.TabIndex = 15;
            this.label10.Text = "g";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(882, 1368);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 81);
            this.label11.TabIndex = 14;
            this.label11.Text = "f";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(715, 1368);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 81);
            this.label12.TabIndex = 13;
            this.label12.Text = "e";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(1324, 120);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 81);
            this.label13.TabIndex = 17;
            this.label13.Text = "8";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(1329, 293);
            this.label14.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 81);
            this.label14.TabIndex = 18;
            this.label14.Text = "7";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(1334, 608);
            this.label15.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 81);
            this.label15.TabIndex = 20;
            this.label15.Text = "5";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(1329, 458);
            this.label16.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 81);
            this.label16.TabIndex = 19;
            this.label16.Text = "6";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label17.Location = new System.Drawing.Point(1339, 941);
            this.label17.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 81);
            this.label17.TabIndex = 22;
            this.label17.Text = "3";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label18.Location = new System.Drawing.Point(1334, 768);
            this.label18.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(70, 81);
            this.label18.TabIndex = 21;
            this.label18.Text = "4";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label19.Location = new System.Drawing.Point(1344, 1261);
            this.label19.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 81);
            this.label19.TabIndex = 24;
            this.label19.Text = "1";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label20.Location = new System.Drawing.Point(1339, 1088);
            this.label20.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(70, 81);
            this.label20.TabIndex = 23;
            this.label20.Text = "2";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1826, 1378);
            this.button4.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(153, 46);
            this.button4.TabIndex = 25;
            this.button4.Text = "RESET";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ResetButtonClicked);
            // 
            // chooseOpponentComboBox
            // 
            this.chooseOpponentComboBox.FormattingEnabled = true;
            this.chooseOpponentComboBox.Items.AddRange(new object[] {
            "Prvák matematik",
            "Prvák informatik",
            "Bakalář",
            "Magistr ",
            "Doktor",
            "Martin Pergel",
            "Náhodný člověk z internetu",
            "Jen tak si tahat"});
            this.chooseOpponentComboBox.Location = new System.Drawing.Point(1539, 19);
            this.chooseOpponentComboBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chooseOpponentComboBox.Name = "chooseOpponentComboBox";
            this.chooseOpponentComboBox.Size = new System.Drawing.Size(366, 40);
            this.chooseOpponentComboBox.TabIndex = 26;
            this.chooseOpponentComboBox.Text = "Jen tak si tahat";
            this.chooseOpponentComboBox.SelectedValueChanged += new System.EventHandler(this.ChooseOpponentComboBoxValueChanged);
            // 
            // showValidMovesCheckBox
            // 
            this.showValidMovesCheckBox.AutoSize = true;
            this.showValidMovesCheckBox.Location = new System.Drawing.Point(1573, 1386);
            this.showValidMovesCheckBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.showValidMovesCheckBox.Name = "showValidMovesCheckBox";
            this.showValidMovesCheckBox.Size = new System.Drawing.Size(238, 36);
            this.showValidMovesCheckBox.TabIndex = 27;
            this.showValidMovesCheckBox.Text = "Show valid moves";
            this.showValidMovesCheckBox.UseVisualStyleBackColor = true;
            this.showValidMovesCheckBox.CheckedChanged += new System.EventHandler(this.ShowValidMovesCheckBoxChanged);
            // 
            // blackDiscardedPiecesPanel
            // 
            this.blackDiscardedPiecesPanel.Location = new System.Drawing.Point(1921, 120);
            this.blackDiscardedPiecesPanel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.blackDiscardedPiecesPanel.Name = "blackDiscardedPiecesPanel";
            this.blackDiscardedPiecesPanel.Size = new System.Drawing.Size(424, 502);
            this.blackDiscardedPiecesPanel.TabIndex = 28;
            // 
            // whiteDiscardedPiecesPanel
            // 
            this.whiteDiscardedPiecesPanel.Location = new System.Drawing.Point(1921, 864);
            this.whiteDiscardedPiecesPanel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.whiteDiscardedPiecesPanel.Name = "whiteDiscardedPiecesPanel";
            this.whiteDiscardedPiecesPanel.Size = new System.Drawing.Size(577, 514);
            this.whiteDiscardedPiecesPanel.TabIndex = 29;
            // 
            // ChoosePlayerColorCheckBox
            // 
            this.ChoosePlayerColorCheckBox.AutoSize = true;
            this.ChoosePlayerColorCheckBox.Location = new System.Drawing.Point(1575, 1427);
            this.ChoosePlayerColorCheckBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ChoosePlayerColorCheckBox.Name = "ChoosePlayerColorCheckBox";
            this.ChoosePlayerColorCheckBox.Size = new System.Drawing.Size(391, 36);
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
            this.gameInfoLabel.Location = new System.Drawing.Point(200, 26);
            this.gameInfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.gameInfoLabel.Name = "gameInfoLabel";
            this.gameInfoLabel.Size = new System.Drawing.Size(78, 32);
            this.gameInfoLabel.TabIndex = 31;
            this.gameInfoLabel.Text = "label1";
            this.gameInfoLabel.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1410, 66);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 46);
            this.button1.TabIndex = 32;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendConnectedStatusTimer
            // 
            this.sendConnectedStatusTimer.Interval = 2000;
            this.sendConnectedStatusTimer.Tick += new System.EventHandler(this.stillConnectedTimerTick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2642, 1637);
            this.Controls.Add(this.button1);
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
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.opponentNameLabel);
            this.Location = new System.Drawing.Point(200, 200);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
        private Label label3;
        private Label label4;
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
        private Button button1;
        private System.Windows.Forms.Timer sendConnectedStatusTimer;
    }
}