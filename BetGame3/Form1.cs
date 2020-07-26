using BetGame3.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetGame3
{
    public partial class Form1 : Form
    {
        Dog[] dogs = new Dog[4];
        Punter[] players = new Punter[3];
        Dog winnerDog;
        Timer[] timers = new Timer[4];

        public Form1()
        {
            InitializeComponent();
            InitializeGameSettings();
        }

        private void InitializeGameSettings()
        {
            // Dog Details
            dogs[0] = new Dog() { DogName = "Dog 1", FinishPoint = 40, MyPictureBox = pictureDog1 };
            dogs[1] = new Dog() { DogName = "Dog 2", FinishPoint = 40, MyPictureBox = pictureDog2 };
            dogs[2] = new Dog() { DogName = "Dog 3", FinishPoint = 40, MyPictureBox = pictureDog3 };
            dogs[3] = new Dog() { DogName = "Dog 4", FinishPoint = 40, MyPictureBox = pictureDog4 };

            // Player Details
            players[0] = Factory.GetPunterInstance("Ava");
            players[1] = Factory.GetPunterInstance("Harper");
            players[2] = Factory.GetPunterInstance("Willow");

            players[0].MyRadioButton = radioPunter1;
            players[0].MyText = textPunter1;
            players[0].MyRadioButton.Text = players[0].Name;


            players[1].MyRadioButton = radioPunter2;
            players[1].MyText = textPunter2;
            players[1].MyRadioButton.Text = players[1].Name;


            players[2].MyRadioButton = radioPunter3;
            players[2].MyText = textPunter3;
            players[2].MyRadioButton.Text = players[2].Name;

            
        }


        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text.Contains("Place"))
            {
                int count = 0;
                int total_active = 0;
                foreach (Punter player in players)
                {
                    if (!player.Busted)
                    {
                        total_active++;
                        if (player.MyRadioButton.Checked)
                        {
                            if (player.MyBet == null)
                            {
                                int number = (int)numericDogNumber.Value;
                                int amount = (int)numericBetAmount.Value;
                                bool alreadyPlaced = false;
                                foreach (Punter pla in players)
                                {
                                    if (pla.MyBet != null && pla.MyBet.Dog == dogs[number - 1])
                                    {
                                        alreadyPlaced = true;
                                        break;
                                    }
                                }
                                if (alreadyPlaced)
                                {
                                    MessageBox.Show("This Cycle Dog is Already Taken...");
                                }
                                else
                                {
                                    player.MyBet = new Bet() { Amount = amount, Dog = dogs[number - 1] };
                                }

                            }
                            else
                            {
                                MessageBox.Show("You Already Place Bet for " + player.Name);
                            }
                        }
                        if (player.MyBet != null)
                        {
                            count++;
                        }
                    }
                }
                SetDetails();
                if (count == total_active)
                {
                    btnAction.Text = "Begin The Race";
                    panelControls.Enabled = false;
                }
            }
            else if (btnAction.Text.Contains("Begin"))
            {
                for (int index = 0; index < timers.Length; index++)
                {
                    timers[index] = new Timer();
                    timers[index].Interval = 15;
                    timers[index].Tick += Timer_Tick;
                }
                for (int index = 0; index < timers.Length; index++)
                {
                    timers[index].Start();
                }
            }
            else if (btnAction.Text.Contains("End"))
            {
                MessageBox.Show("Game Over!!!");
                Application.Exit();
            }
        }

        private void Timer_Tick(object sender, EventArgs args)
        {
            if (sender is Timer)
            {
                int index = -1;
                Timer timer = sender as Timer;
                for( int i = 0; i < timers.Length; i++)
                {
                    if( timer == timers[i])
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                {
                    PictureBox picture = dogs[index].MyPictureBox;
                    if (picture.Location.X < dogs[index].FinishPoint)
                    {
                        if (winnerDog == null)
                        {
                            winnerDog = dogs[index];
                        }
                        for(int i = 0; i < timers.Length; i++)
                        {
                            timers[i].Stop();
                        }
                    }
                    else
                    {
                        int jump = new Random().Next(1, 15);
                        picture.Location = new Point(picture.Location.X - jump, picture.Location.Y);
                    }
                }
            }
            if (winnerDog != null)
            {
                MessageBox.Show("Hurray!!! " + winnerDog.DogName + " is Won...");
                SetDetails();
                foreach (Punter player in players)
                {
                    if (player.MyBet != null)
                    {
                        if (player.MyBet.Dog == winnerDog)
                        {
                            player.Cash += player.MyBet.Amount;
                            player.MyText.Text = player.Name + " Won and now has $" + player.Cash;                            
                        }
                        else
                        {
                            player.Cash -= player.MyBet.Amount;
                            if (player.Cash == 0)
                            {
                                player.MyText.Text = "BUSTED";
                                player.Busted = true;
                                player.MyRadioButton.Enabled = false;
                            }
                            else
                            {
                                player.MyText.Text = player.Name + " Lost and now has $" + player.Cash;
                            }
                        }
                    }
                }
                winnerDog = null;
                for(int i =0; i < timers.Length; i++)
                {
                    timers[i] = null;
                }
                int count = 0;
                foreach (Punter player in players)
                {
                    if (player.Busted)
                    {
                        count++;
                    }
                    if (player.MyRadioButton.Enabled && player.MyRadioButton.Checked)
                    {
                        labelMaxBet.Text = player.Name + " Max Bet is $" + player.Cash;
                        numericBetAmount.Maximum = player.Cash;
                        numericBetAmount.Minimum = 1;
                    }
                    player.MyBet = null;                    
                }
                if (count == players.Length)
                {
                    btnAction.Text = "End Game";

                }
                else
                {
                    panelControls.Enabled = true;
                }
                foreach (Dog dog in dogs)
                {
                    dog.MyPictureBox.Location = new Point(810, dog.MyPictureBox.Location.Y);
                }
            }
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            SetDetails();
        }

        private void SetDetails()
        {
            foreach (Punter player in players)
            {
                if (player.Busted)
                {
                    player.MyText.Text = "BUSTED";
                }
                else
                {
                    if (player.MyBet == null)
                    {
                        player.MyText.Text = player.Name + " hasn't placed a bet";
                    }
                    else
                    {
                        player.MyText.Text = player.Name + " bets $" + player.MyBet.Amount + " on " + player.MyBet.Dog.DogName;
                    }
                    if (player.MyRadioButton.Checked)
                    {
                        labelMaxBet.Text = player.Name + " Max Bet Amount is $" + player.Cash.ToString();
                        btnAction.Text = "Place Bet for " + player.Name;
                        labelBet.Text = player.Name + " Bet Amount is $";
                        labelBetDog.Text = player.Name + " Bet on Dog No";
                        numericBetAmount.Maximum = player.Cash;
                        numericBetAmount.Value = 1;
                    }
                }
            }
        }
    }
}
