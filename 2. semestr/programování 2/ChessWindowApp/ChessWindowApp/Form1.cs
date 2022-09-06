using chessengine_console;

namespace ChessWindowApp
{
    public partial class Form1 : Form
    {

        ChessBoard board = new ChessBoard();
        public Button[,] brnGrid = new Button[8, 8];

        public Form1()
        {
            InitializeComponent();

            this.printButtonGrid();
        }

        private void printButtonGrid()
        {
            int buttonSize = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    brnGrid[i, j] = new Button();
                    brnGrid[i, j].Width = buttonSize;
                    brnGrid[i, j].Height = buttonSize;

                    brnGrid[i, j].Click += this.gridButtonClick;

                    panel1.Controls.Add(brnGrid[i, j]);

                    brnGrid[i, j].Location = new Point(i * buttonSize, j * buttonSize);

                    brnGrid[i, j].Text = i + "|" + j;
                }
            }
        }

        private void gridButtonClick(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button68_Click(object sender, EventArgs e)
        {

        }

        private void resetGameButton_Click(object sender, EventArgs e)
        {
            board = new ChessBoard();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                }
            }
        }
    }
}