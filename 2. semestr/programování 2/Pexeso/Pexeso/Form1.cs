using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexeso
{
    public partial class Pexeso : Form
    {
        Random numberGenerator = new Random();

        Label firstLabel = null;
        Label secondLabel = null;
        bool blockUserMoves = false;

        List<string> pictures = new List<string>()
        {
            "orange", "#ffa200",
            "grey", "#2a2c30",
            "turquoise", "#00f7ff",
            "yellow", "#ffee00",
            "pink", "#ff006f",
            "brown", "#402626"
        };

        Dictionary<string, string> correctPairs = new Dictionary<string, string>()
        {
            {"orange", "#ffa200" },
            {"grey", "#2a2c30" },
            {"turquoise", "#00f7ff" },
            {"yellow", "#ffee00" },
            {"pink", "#ff006f" },
            {"brown", "#402626" }
        };

        List<Label> foundLabels = new List<Label>();
        int[] playersScore = new int[2] {0, 0};
        int playerOnMove = 0;
        private void generatePictures()
        {
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                int random = numberGenerator.Next(0, pictures.Count);
                Label label = (Label)tableLayoutPanel1.Controls[i];

                label.Text = pictures[random];
                label.ForeColor = Color.Salmon;
                label.BackColor = Color.Salmon;
                pictures.RemoveAt(random);
            }
        }


        public Pexeso()
        {
            InitializeComponent();
            generatePictures();
        }

        private bool allFoundCheck()
        {
            if(foundLabels.Count == 12)
            {
                return true;
            }
            return false;
        }

        private bool checkCorrectPair(Label label1, Label label2)
        {
            string text1 = label1.Text;
            string text2 = label2.Text;

            if (correctPairs.ContainsKey(text1))
            {
                if(correctPairs[text1] == text2)
                {
                    return true;
                }
                return false;
            }
            if (correctPairs.ContainsKey(text2))
            {
                if(correctPairs[text2] == text1)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private void clickOnLabel(object sender, EventArgs e)
        {
            if(blockUserMoves)
            {
                return;
            }
            Label label = sender as Label;
            if (foundLabels.Contains(label))
            {
                return;
            }

            if(firstLabel == null)
            {
                firstLabel = label;
                firstLabel.ForeColor = Color.White;
                return;
            }
            if(firstLabel != label)
            {
                secondLabel = label;
                secondLabel.ForeColor = Color.White;
                blockUserMoves = true;
                playerOnMove += 1;
                playerOnMove %= 2;

            }
            else
            {
                return;
            }
            if(checkCorrectPair(firstLabel, secondLabel))
            {
                foundLabels.Add(firstLabel);
                foundLabels.Add(secondLabel);

                firstLabel.BackColor = Color.Lime;
                secondLabel.BackColor = Color.Lime;

                firstLabel = null;
                secondLabel = null;

                blockUserMoves = false;
                playersScore[playerOnMove] += 1;

                if (allFoundCheck())
                {
                    if(playersScore[0] > playersScore[1])
                    {
                        MessageBox.Show("Player 1 has won");                     
                    }
                    if (playersScore[0] == playersScore[1])
                    {
                        MessageBox.Show("Both players have achieved same score");
                    }
                    else
                    {
                        MessageBox.Show("Player 2 has won");
                    }
                    blockUserMoves = true;
                }
            }
            else
            {          
                timer1.Start();
            }
            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstLabel.ForeColor = Color.Salmon;
            secondLabel.ForeColor = Color.Salmon;

            firstLabel = null;
            secondLabel = null;

            blockUserMoves = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
