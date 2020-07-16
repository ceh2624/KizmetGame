using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kizmet// see if this is in Git, you git. Now show changes?
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        public bool working = false; //Makes player wait to get next score preventing cheating whether accidental or not
        PictureBox[] diPic = new PictureBox[5]; //this puts my picture boxes in an array so they work in a loop
        Button[] scoreButton = new Button[15]; 
        Button[] holdButton = new Button[5]; //so I can reset all the hold buttons to green
        private void holdStatusChange(Button but)
        {
            string butName = but.Name.ToString();
            butName = butName[butName.Length - 1].ToString();
            int butID = Convert.ToInt32(butName);
            if(but.Text == "HOLD")
            {
                but.Text = "HELD";
                but.BackColor = Color.Red;
                Dice.held[butID] = true;
            }
            else
            {
                but.Text = "HOLD";
                but.BackColor = Color.LimeGreen;
                Dice.held[butID] = false; 
            }
        }


        private void btnHold1_Click(object sender, EventArgs e)
        {
            Button clickBtn = (Button)sender;
            holdStatusChange(clickBtn);
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            working = false;
            for (int x = 1; x <= 5; x++)
            {
                if (Dice.held[x] == false && Dice.throws > 0)
                {
                    Dice.cubes[x] = Dice.RollCubes();
                    switch (Dice.cubes[x])      // jpegs for dice faces are oneB, twoR... and so on are stored in "Resources.resx".
                    {
                        case 1:
                            diPic[x-1].Image = Properties.Resources.oneB;
                            break;
                        case 2:
                            diPic[x-1].Image = Properties.Resources.twoR;
                            break;
                        case 3:
                            diPic[x-1].Image = Properties.Resources.threeG;
                            break;
                        case 4:
                            diPic[x-1].Image = Properties.Resources.fourG;
                            break;
                        case 5:
                            diPic[x-1].Image = Properties.Resources.fiveR;
                            break;
                        case 6:
                            diPic[x-1].Image = Properties.Resources.sixB;
                            break;
                    }

                }
            }
            Dice.throws--;
            if(Dice.throws != 1)
                lblThrows.Text = Dice.throws.ToString() + " Rolls Left";
            else
                lblThrows.Text = Dice.throws.ToString() + " Roll Left";

            if (Dice.throws == 0)
            {
                btnRoll.Enabled = false;
                lblThrows.Focus();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            diPic[0] = pictureBox1;  //build the pictureBox array to work in loops!
            diPic[1] = pictureBox2;
            diPic[2] = pictureBox3;
            diPic[3] = pictureBox4;
            diPic[4] = pictureBox5;
            holdButton[0] = btnHold1;
            holdButton[1] = btnHold2;
            holdButton[2] = btnHold3;
            holdButton[3] = btnHold4;
            holdButton[4] = btnHold5;
            scoreButton[0] = btnOnes;
            scoreButton[1] = btnTwos;
            scoreButton[2] = btnThrees;
            scoreButton[3] = btnFours;
            scoreButton[4] = btnFives;
            scoreButton[5] = btnSixes;
            scoreButton[6] = btnTwoPair;
            scoreButton[7] = btnThreeKind;
            scoreButton[8] = btnStraight;
            scoreButton[9] = btnFlush;
            scoreButton[10] = btnFullHouseColour;
            scoreButton[11] = btnFullHouse;
            scoreButton[12] = btnFourKind;
            scoreButton[13] = btnFiveKind;
            scoreButton[14] = btnChance;
            lblGT.Text = Score.totalScore.ToString();
        }

        private void diReset() //blanks out dice after selecting score.
        {
            for (int D = 1; D <= 5; D++)
            {
                Dice.cubes[D] = 0;
                diPic[D - 1].Image = Properties.Resources.blank;
                Dice.throws = 3;
                btnRoll.Enabled = true;
                Dice.held[D] = false;
            }
            for(int h = 0; h <= 4; h++)
            {
                holdButton[h].Text = "HOLD";
                holdButton[h].BackColor = Color.LimeGreen;
            }
            btnRoll.Focus();
            lblThrows.Text = Dice.throws.ToString() + " Rolls Left";
            lblGT.Text = Score.tGT.ToString();
            Score.numOfOnes = 0;
            Score.numOfTwos = 0;
            Score.numOfThrees = 0;
            Score.numOfFours = 0;
            Score.numOfFives = 0;
            Score.numOfSixes = 0;
        }


        private void btnFiveKind_Click(object sender, EventArgs e)// 5 of a kind or Kizmet
        {
            if (working) return; //Won't run until dice are rolled after each turn
            if (Score.CheckKizmet())
            {
                Score.tKizmet = (Dice.cubes[1] * 5) + 50;
                Score.totalScore += Score.tKizmet;
            }
            else
            {
                Score.tKizmet = 0;
                //Score.totalScore = 1; just for testing!
            }
            lbl5Kind.Text = Score.tKizmet.ToString();
            btnFiveKind.Enabled = false;
            btnFiveKind.Text = "#";
            diReset();
            sumBottomHalf(Score.tKizmet);
            working = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            lblKflash.Visible = false;
        }

        private void lblThrows_DoubleClick(object sender, EventArgs e) //a wee easter egg
        {
            MessageBox.Show("Reset the lot!");
        }

        private void sumTopHalf(int numFromTop)
        {
            Score.tTop += numFromTop;
            setTopBonus();
            Score.tGT = Score.tTop + Score.topBonus + Score.tBottom + Score.kPlusPoints;
            lblTotTop.Text = Score.tTop.ToString();
            lblGT.Text = Score.tGT.ToString();
        }

        private void sumBottomHalf(int numFromBottom)
        {
            Score.tBottom += numFromBottom;
            Score.tGT = Score.tTop + Score.topBonus + Score.tBottom + Score.kPlusPoints;
            lblGT.Text = Score.tGT.ToString();
        }

        private void btnOnes_Click(object sender, EventArgs e)
        {
            if (working) return; //Won't run until dice are rolled after each turn
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tOnes = 5;
                Score.kPlusPoints += 25;
                //Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d1 = 1; d1 <= 5; d1++)
                {
                    if (Dice.cubes[d1] == 1)
                        Score.tOnes += 1;
                }

            }
            lblOnes.Text = Score.tOnes.ToString();
            diReset();
            btnOnes.Enabled = false;
            btnOnes.Text = "#";
            working = true;
            sumTopHalf(Score.tOnes);
        }

        private void btnTwos_Click(object sender, EventArgs e)
        {
            if (working) return; //Won't run until dice are rolled after each turn
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tTwos = 10;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d2 = 1; d2 <= 5; d2++)
                {
                    if (Dice.cubes[d2] == 2)
                        Score.tTwos += 2;
                }

            }
            lblTwos.Text = Score.tTwos.ToString();
            diReset();
            btnTwos.Enabled = false;
            btnTwos.Text = "#";
            working = true;
            sumTopHalf(Score.tTwos);
        }

        private void btnThrees_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tThrees = 15;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d3 = 1; d3 <= 5; d3++)
                {
                    if (Dice.cubes[d3] == 3)
                        Score.tThrees += 3;
                }

            }
            lblThrees.Text = Score.tThrees.ToString();
            diReset();
            btnThrees.Enabled = false;
            btnThrees.Text = "#";
            working = true;
            sumTopHalf(Score.tThrees);
        }

        private void btnFours_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFours = 20;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d4 = 1; d4 <= 5; d4++)
                {
                    if (Dice.cubes[d4] == 4)
                        Score.tFours += 4;
                }

            }
            lblFours.Text = Score.tFours.ToString();
            diReset();
            btnFours.Enabled = false;
            btnFours.Text = "#";
            working = true;
            sumTopHalf(Score.tFours);
        }

        private void btnFives_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFives = 25;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d5 = 1; d5 <= 5; d5++)
                {
                    if (Dice.cubes[d5] == 5)
                        Score.tFives += 5;
                }

            }
            lblFives.Text = Score.tFives.ToString();
            diReset();
            btnFives.Enabled = false;
            btnFives.Text = "#";
            working = true;
            sumTopHalf(Score.tFives);
        }

        private void btnSixes_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tSixes = 30;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for (int d6 = 1; d6 <= 5; d6++)
                {
                    if (Dice.cubes[d6] == 6)
                        Score.tSixes += 6;
                }

            }
            lblSixes.Text = Score.tSixes.ToString();
            diReset();
            btnSixes.Enabled = false;
            btnSixes.Text = "#";
            working = true;
            sumTopHalf(Score.tSixes);
        }

        private void setTopBonus()
        {
            if (Score.tTop > 62 && Score.tTop < 71)
                Score.topBonus = 35;
            if (Score.tTop > 71 && Score.tTop < 78)
                Score.topBonus = 55;
            if (Score.tTop > 78)
                Score.topBonus = 75;

            lblTotTop.Text = Score.tTop.ToString();
            lblBonus.Text = Score.topBonus.ToString();

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Score.kPlusPoints.ToString();
        }

        private void checkGTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Score.tGT.ToString();
        }

        private void checkBonusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Score.topBonus.ToString();
        }

        private void checkTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Score.tTop.ToString();
        }

        private void checkBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = Score.tBottom.ToString();
        }

        private void btnChance_Click(object sender, EventArgs e)
        {
            if (working) return; //Won't run until dice are rolled after each turn
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tChance = (Dice.cubes[1] * 5) + 50;
                Score.totalScore += Score.tChance;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                for(int dCh = 1; dCh <=5; dCh++)
                    Score.tChance += Dice.cubes[dCh];
            }    
            lblChance.Text = Score.tChance.ToString();
            btnChance.Enabled = false;
            btnChance.Text = "#";
            diReset();
            sumBottomHalf(Score.tChance);
            working = true;
        }

        private void btnTwoPair_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tTwoPair = Dice.cubes[1] * 5;
                Score.totalScore += Score.tTwoPair;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (((Score.numOfOnes > 1 && Score.numOfSixes > 1) ||
                    (Score.numOfTwos > 1 && Score.numOfFives > 1) ||
                    (Score.numOfThrees > 1 && Score.numOfFours > 1)) || //big or
                    (Score.numOfOnes > 3 || Score.numOfTwos > 3 || Score.numOfThrees > 3 ||
                    Score.numOfFours > 3 || Score.numOfFives > 3 || Score.numOfSixes > 3))
                {
                    for (int d2p = 1; d2p <= 5; d2p++)
                        Score.tTwoPair += Dice.cubes[d2p];
                }
            }   
            lbl2Pair.Text = Score.tTwoPair.ToString();
            btnTwoPair.Enabled = false;
            btnTwoPair.Text = "#";
            diReset();
            sumBottomHalf(Score.tTwoPair);
            working = true;
        }

        private void btnThreeKind_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tThreeKind = Dice.cubes[1] * 5;
                Score.totalScore += Score.tThreeKind;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (Score.numOfOnes > 2 || Score.numOfTwos > 2 || Score.numOfThrees > 2 ||
                   Score.numOfFours > 2 || Score.numOfFives > 2 || Score.numOfSixes > 2)
                {
                    for (int d3k = 1; d3k <= 5; d3k++)
                        Score.tThreeKind += Dice.cubes[d3k];
                }
             }   //else Score.tThreeKind = 0;

            lbl3Kind.Text = Score.tThreeKind.ToString();
            btnThreeKind.Enabled = false;
            btnThreeKind.Text = "#";
            diReset();
            sumBottomHalf(Score.tThreeKind);
            working = true;
        }

        private void btnStraight_Click(object sender, EventArgs e)
        {
            if (working) return;
            if(Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tStraight = 0;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if ((Score.numOfTwos == 1 && Score.numOfThrees == 1 && Score.numOfFours == 1 && Score.numOfFives == 1) &&
                    (Score.numOfOnes == 1 || Score.numOfSixes == 1))
                    Score.tStraight = 30;
            }
            lblStraight.Text = Score.tStraight.ToString();
            btnStraight.Enabled = false;
            btnStraight.Text = "#";
            diReset();
            sumBottomHalf(Score.tStraight);
            working = true;
        }

        private void btnFlush_Click(object sender, EventArgs e)
        {
            if (working) return;
            if(Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFlush = 35;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (Score.numOfOnes + Score.numOfSixes == 5 || Score.numOfTwos + Score.numOfFives == 5 || Score.numOfThrees + Score.numOfFours == 5)
                    Score.tFlush = 35;
            }
            lblFlush.Text = Score.tFlush.ToString();
            btnFlush.Enabled = false;
            btnFlush.Text = "#";
            diReset();
            sumBottomHalf(Score.tFlush);
            working = true;
        }

        private void btnFourKind_Click(object sender, EventArgs e)
        {
            if (working) return;
            if(Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFourKind = Dice.cubes[1] + 25;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (Score.numOfOnes > 3 || Score.numOfTwos > 3 || Score.numOfThrees > 3 ||
                   Score.numOfFours > 3 || Score.numOfFives > 3 || Score.numOfSixes > 3)
                {
                    for (int d4k = 1; d4k <= 5; d4k++)
                        Score.tFourKind += Dice.cubes[d4k];

                    Score.tFourKind += 25;
                }
            }
            lbl4Kind.Text = Score.tFourKind.ToString();
            btnFourKind.Enabled = false;
            btnFourKind.Text = "#";
            diReset();
            sumBottomHalf(Score.tFourKind);
            working = true;
        }

        private void btnFullHouse_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFull = 0;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (isFullHouse())
                {
                    for (int fh = 1; fh <= 5; fh++)
                        Score.tFull += Dice.cubes[fh];

                    Score.tFull += 15;
                }
            }
            lblFullHouse.Text = Score.tFull.ToString();
            btnFullHouse.Enabled = false;
            btnFullHouse.Text = "#";
            diReset();
            sumBottomHalf(Score.tFull);
            working = true;
        }

        private void btnFullHouseColour_Click(object sender, EventArgs e)
        {
            if (working) return;
            if (Score.CheckKizmet() && Score.kPlus > 1)
            {
                Score.tFullColour = 0;
                Score.kPlusPoints += 25;
                Score.totalScore += 25;
                lblKbonus.Text = Score.kPlusPoints.ToString() + " Extra Kizmet Points!";
                lblKflash.Visible = true;
                timer1.Start();
                lblKbonus.Visible = true;
            }
            else
            {
                Dice.getNumbers();
                if (((Score.numOfOnes == 2 && Score.numOfSixes == 3) || (Score.numOfOnes == 3 && Score.numOfSixes == 2)) ||
                    ((Score.numOfTwos == 2 && Score.numOfFives == 3) || (Score.numOfTwos == 3 && Score.numOfFives == 2)) ||
                    ((Score.numOfThrees == 2 && Score.numOfFours == 3)) || (Score.numOfThrees == 3 && Score.numOfFours == 2))
                {
                    for (int fhc = 1; fhc <= 5; fhc++)
                        Score.tFullColour += Dice.cubes[fhc];

                    Score.tFullColour += 20;
                }
                lblFullHouseColour.Text = Score.tFullColour.ToString();
                btnFullHouseColour.Enabled = false;
                btnFullHouseColour.Text = "#";
                diReset();
                sumBottomHalf(Score.tFullColour);
                working = true;
            }

        }
        private bool isFullHouse()
        {
            bool isFH = false;
            if (Score.numOfOnes == 2 && Score.numOfTwos == 3) isFH = true;
            if (Score.numOfOnes == 2 && Score.numOfThrees == 3) isFH = true;
            if (Score.numOfOnes == 2 && Score.numOfFours == 3) isFH = true;
            if (Score.numOfOnes == 2 && Score.numOfFives == 3) isFH = true;
            if (Score.numOfOnes == 2 && Score.numOfSixes == 3) isFH = true;
            if (Score.numOfOnes == 3 && Score.numOfTwos == 2) isFH = true;
            if (Score.numOfOnes == 3 && Score.numOfThrees == 2) isFH = true;
            if (Score.numOfOnes == 3 && Score.numOfFours == 2) isFH = true;
            if (Score.numOfOnes == 3 && Score.numOfFives == 2) isFH = true;
            if (Score.numOfOnes == 3 && Score.numOfSixes == 2) isFH = true;
            if (Score.numOfTwos == 2 && Score.numOfThrees == 3) isFH = true;
            if (Score.numOfTwos == 2 && Score.numOfFours == 3) isFH = true;
            if (Score.numOfTwos == 2 && Score.numOfFives == 3) isFH = true;
            if (Score.numOfTwos == 2 && Score.numOfSixes == 3) isFH = true;
            if (Score.numOfTwos == 3 && Score.numOfThrees == 2) isFH = true;
            if (Score.numOfTwos == 3 && Score.numOfFours == 2) isFH = true;
            if (Score.numOfTwos == 3 && Score.numOfFives == 2) isFH = true;
            if (Score.numOfTwos == 3 && Score.numOfSixes == 2) isFH = true;
            if (Score.numOfThrees == 2 && Score.numOfFours == 3) isFH = true;
            if (Score.numOfThrees == 2 && Score.numOfFives == 3) isFH = true;
            if (Score.numOfThrees == 2 && Score.numOfSixes == 3) isFH = true;
            if (Score.numOfThrees == 3 && Score.numOfFours == 2) isFH = true;
            if (Score.numOfThrees == 3 && Score.numOfFives == 2) isFH = true;
            if (Score.numOfThrees == 3 && Score.numOfSixes == 2) isFH = true;
            if (Score.numOfFours == 2 && Score.numOfFives == 3) isFH = true;
            if (Score.numOfFours == 2 && Score.numOfSixes == 3) isFH = true;
            if (Score.numOfFours == 3 && Score.numOfFives == 2) isFH = true;
            if (Score.numOfFours == 3 && Score.numOfSixes == 2) isFH = true;
            if (Score.numOfFives == 2 && Score.numOfSixes == 3) isFH = true;
            if (Score.numOfFives == 3 && Score.numOfSixes == 2) isFH = true;
            return isFH;
        }
    }
    public class Dice
    {
        //class to roll dice and store their values 
        public static int[] cubes = new int[6];
        public static bool[] held = new bool[6];
        public static int throws = 3;
        private static readonly Random rand = new Random();

        public static int RollCubes()
        {

            int dice = rand.Next(1, 7);
            return dice;
        }

        public static void getNumbers()
        {
            for(int diNum = 1; diNum <= 5; diNum++)
            {
                switch (Dice.cubes[diNum])
                 {   
                    case 1:
                        Score.numOfOnes += 1;
                        break;
                    case 2:
                        Score.numOfTwos += 1;
                        break;
                    case 3:
                        Score.numOfThrees += 1;
                        break;
                    case 4:
                        Score.numOfFours += 1;
                        break;
                    case 5:
                        Score.numOfFives += 1;
                        break;
                    case 6:
                        Score.numOfSixes += 1;
                        break;
                }
            }
        }
    }
    public class Score
    {
        public static int kPlus;
        public static int kPlusPoints;
        public static int totalScore;
        public static int tOnes;
        public static int tTwos;
        public static int tThrees;
        public static int tFours;
        public static int tFives;
        public static int tSixes;
        public static int tTwoPair;
        public static int tThreeKind;
        public static int tStraight;
        public static int tFlush;
        public static int tFull;
        public static int tFullColour;
        public static int tFourKind;
        public static int tChance;
        public static int tKizmet;
        public static int tTop;
        public static int tBottom;
        public static int topBonus;
        public static int tTopGT;
        public static int tGT;
        public static int numOfOnes;
        public static int numOfTwos;
        public static int numOfThrees;
        public static int numOfFours;
        public static int numOfFives;
        public static int numOfSixes;

        public static bool CheckKizmet()
        {
            if (Dice.cubes[1] == Dice.cubes[2] &&
               Dice.cubes[2] == Dice.cubes[3] &&
               Dice.cubes[3] == Dice.cubes[4] &&
               Dice.cubes[4] == Dice.cubes[5])
                 kPlus++;
            else
                return false;

            return true;
        }
    }
}
