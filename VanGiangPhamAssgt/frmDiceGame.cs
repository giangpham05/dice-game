#region Using Statments
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
#endregion
namespace VanGiangPhamAssgt
{
    public partial class frm_DiceGame : Form
    {
        #region DECLERATION
        // To make it easier to modify each control in the game, we create controls that we are going
        // to use here rather than drag and drop them from the toolbox. There are so many control we will use in this game

        private Image[] diceImages;                     // store images that represent the numbers on dice
        private int[] iDicDef = new int[6];             // store default numbers on dice when the game starts.
        private static Random
            randTurn,                                   // randomly generates players' turn
            randRoll;                                   // randomly generates number on each die
        private int
            iGoalScore = 50,                            // represent preset of goal score.
            turn;                                       // a player's turn when randTurn is intialized
        private int iNumOfDice;                         // number of dice players choose to roll, by default it is set to 6
        private Player player1, player2;                // players, it could be a user playing against computer or multiple human players
        private bool isPlayedWithComp = false,          // indicates that the player chooses to play  with computer
            isMultiPlayer = false;                      // indicates 2 players play against each other
        private int tabIndex = 0;                       // tab index of controls

        // Menu controls for MENU INTERFACE:
        private PictureBox picBox_Menu = new PictureBox();  // picturebox containing menu buttons
        private Label lbl_numDice_Menu = new Label();       // label displaying information in the menu
        private TextBox txt_NumDice_Menu = new TextBox();   // textbox providing interface for players to enter goal score
        private Label lbl_PlayerDeterminant = new Label();  // when the player chooses to play with computer, this label will be shown to indicate
                                                            // player1 is the human player
        /*
         * butMenu[0] = START, butMenu[1] = QUIT, butMenu[2] = PLAY WITH COMPUTER,  butMenu[3] = MULTIPLAYER
         * butMenu[5] = BACK TO START, butMenu[6] = OK, butMenu[5] = BACK TO PLAYER MODE
         */
        private Button[] butMenu = new Button[7];
        //Stores click event, there are 7 buttons therefor 7 EventHandler needed
        private EventHandler[] butMenuHanders = new EventHandler[7]; 

        /*
         * radButChooseDice[0] = Choose 1 die to roll
         * radButChooseDice[1] = Choose 2 dice to roll
         * ..............
         * radButChooseDice[5] = Choose 6 dice to roll
         */
        private RadioButton[] radButChooseDice = new RadioButton[6];
        // Stores click event, there are 6 radio buttons therefor 6 EventHandler needed
        private EventHandler[] radHandler = new EventHandler[6];

        // Controlsfor GAME PLAY INTERFACE:
        // Picboxes to display players'turn
        private PictureBox picBox_P1Roll = new PictureBox();     // An image to display player1's turn
        private PictureBox picBox_P2Roll = new PictureBox();     // An image to display player2's turn

        private Label lbl_GoalScore_Game = new Label();         // Goal score on the top left of the play game's view
        private Label lbl_NumDice_Game = new Label();           // Number of dice choose on the top right of the play game's view
        private PictureBox dice_Table = new PictureBox();       // the backgound - like a table for dice game in casino
        private PictureBox[] picBoxImages = new PictureBox[6];  // reprents faces of dice, the Image property of these controls will set to the number on dice rolled
                                                                // There is an image array to store faces of dice, once the player roll dice, the numbers come out
                                                                // we will set the images that represent those numbers as image property for these picturebox controls
        private Label[,] lblPlayerScore = new Label[3, 9];      // part of scoresheet storing players' score
        private PictureBox picBox_ScoreSheet = new PictureBox();// Scoresheet showing players' current score
        private Button but_RollDice = new Button();             // roll button letting players roll dice
        //Banner to display whether the player got boojum, dead drop, snake's eyes, snaffle, one 1 or other case
        private PictureBox picBox_Banner = new PictureBox();    // picturebox containing ban_Label below
        private Label ban_Label = new Label();                  // label displaying boojum, dead drop, snake's eyes, snaffle, one 1 or other case

        //Controls for GAME RESULTS INTERFACE:
        //Picboxes to display players' winning
        private PictureBox picBox_P1Win = new PictureBox();     //picturebox containing an image of player1's winning
        private PictureBox picBox_P2Win = new PictureBox();     //picturebox containing an image of player2's winning

        //Button controls letting players whether continue to play or not
        private Button btn_PlayAgain = new Button();            // play again button
        private Button btn_BackToPlayerMode = new Button();     // back to player mode button

        //Picturebox - background for winning screen
        private PictureBox picBox_Bg = new PictureBox();
        
        //Labels showing the number of games won by each player
        Label lbl_GamesWon = new Label();
        Label lbl_P1GamesWon = new Label();
        Label lbl_P2GamesWon = new Label();

        #endregion

        #region INITIALIZATION
        
        public frm_DiceGame()
        {
            InitializeComponent();
            diceImages = new Image[6];
            randTurn = new Random();
            randRoll = new Random();
            //Default roll when game starts, this is just to display dice images.
            for (int i = 0; i < iDicDef.Length; i++)
            {
                iDicDef[i] = randRoll.Next(1, 6 + 1);
            }
            //load images to the default dice array
            diceImages[0] = Properties.Resources.dice1;
            diceImages[1] = Properties.Resources.dice2;
            diceImages[2] = Properties.Resources.dice3;
            diceImages[3] = Properties.Resources.dice4;
            diceImages[4] = Properties.Resources.dice5;
            diceImages[5] = Properties.Resources.dice6;

            //Intialize menu components
            CreateMenuBoard(); 
            ButtonsHandler();
            CreateMenuButtons();
            CreateChooseDiceField();
            CreateTxtDiceField();
            CreatePlayerDeterminant();

            //Intialize game play components
            CreateTable();
            CreatePicScoreSheet();
            DrawScoreSheet();
            
            CreateGoalScoreGame();
            CreateNumOfDiceGame();
            CreateRollButton();
            CreateDicePictureBoxes();

            RadioButHandler();
            CreateRadButtons();

            CreatePicPRoll(picBox_P1Roll);
            CreatePicPRoll(picBox_P2Roll);

            //Intialize game result components
            CreateWinningScreen();
            CreatePicPRoll(picBox_P1Win);
            CreatePicPRoll(picBox_P2Win);
            Create_PlayAgain_BackMode(btn_PlayAgain);
            Create_PlayAgain_BackMode(btn_BackToPlayerMode);

            player1 = new Player(picBox_P1Roll, picBox_P1Win);
            player2 = new Player(picBox_P2Roll, picBox_P2Win);

            CreatPicResultsBan();
            CreateBanLabel();

            Create_Llb_GamesWon(lbl_GamesWon);
            Create_Llb_GamesWon(lbl_P1GamesWon);
            Create_Llb_GamesWon(lbl_P2GamesWon);
            
        }
       

        #endregion

        #region ANIMATION
        public static class Utils // class that uses built-in feature: window animation
        {
            public enum Effects { Roll, Slide, Center, Blend } //user type definiton
            // unmanaged method
            public static void Animate(Control ctl, Effects effect, int msec, int angle)
            {
                int flags = effmap[(int)effect]; //get int value from enum

                if (ctl.Visible) //this is when the control is visible
                {
                    flags |= 0x10000; //flags = flags Or flags = 0x10000 (flags = flags|0x10000)
                    angle += 180;
                }
                else
                {
                    if (ctl.TopLevelControl == ctl) //Gets the parent control that is not parented by another Windows Forms control.
                        flags |= 0x20000;
                    //If the control is not parented on the form and ctl is not a top level window
                    else if (effect == Effects.Blend) throw new ArgumentException();
                }
                flags |= drmap[angle % 360 / 90];
                bool ok = AnimateWindow(ctl.Handle, msec, flags); //test if it succeeds
                if (!ok) throw new Exception("Animation failed");
                ctl.Visible = !ctl.Visible;
            }
            private static int[] drmap = { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //map direction
            private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 }; //

            [DllImport("user32.dll")] // DllImport attribute from System.Runtime namepsace, user32.dll liabrary is used
            
            //T he extern modifier is used to declare a method that is implemented externally
            //source: https://msdn.microsoft.com/en-us/library/ms632669(VS.85).aspx
            // http://www.autohotkey.com/board/topic/29991-animatewindow-dllcall-test-script-with-code-generator/
            // If the function succeeds, the return value is nonzero, 0 otherwise
            private static extern bool AnimateWindow(
            IntPtr handle,  // A handle to the window to animate.
            int msec,       // The time it takes to play the animation, in milliseconds. Typically, an animation takes 200 milliseconds to play.
            int flags);     // Type: DWORD: The type of animation. This parameter can be one or more of the following values.
                            // Note that, by default, these flags take effect when showing a window.
                            // To take effect when hiding a window, use AW_HIDE and a logical OR operator with the appropriate flags.
                            // effect values: 
                            /*
                                AW_HIDE	:= 0x10000
                                AW_ACTIVATE := 0x20000
                                AW_CENTER := 0x10
                                AW_BLEND := 0x80000
                                AW_SLIDE := 0x40000
                             */

        } //End class Utils
        #endregion

        #region MENU INTERFACE
        //Menu board:
        //this will create a picturebox that contains menu buttons
        private void CreateMenuBoard()
        {
            this.picBox_Menu.BackColor = System.Drawing.Color.Transparent;
            this.picBox_Menu.Image = VanGiangPhamAssgt.Properties.Resources.menuSheet;
            this.picBox_Menu.Location = new System.Drawing.Point(634, -2);
            this.picBox_Menu.Size = new System.Drawing.Size(361, 514);
            this.picBox_Menu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox_Menu.TabIndex = tabIndex; tabIndex++;
            this.picBox_Menu.TabStop = false;
            this.Controls.Add(this.picBox_Menu);
        }
        // initialize event handler of each menu button to the butMenuHanders array
        // we will access these event handler when we create the menu buttons
        private void ButtonsHandler()
        {
            butMenuHanders[0] = this.btn_START_Click;
            butMenuHanders[1] = this.btn_QUIT_Click;
            butMenuHanders[2] = this.btn_MULTIPLAYER_Click;
            butMenuHanders[3] = this.btn_PLAY_COMPUTER_Click;
            butMenuHanders[4] = this.btn_BACK_TO_START_Click;
            butMenuHanders[5] = this.btn_OK_Click;
            butMenuHanders[6] = this.btn_BACK_TO_PLAYER_MODE_Click;

        }
        //create menu buttons
        private void CreateMenuButtons()
        {
            // there are 7 buttons for the menu buttons, storing in order below:
            string[] btnText = new String[7] { "START", "QUIT", "MULTIPLAYER", "PLAY WITH COMPUTER", "BACK TO START", "OK", "BACK TO PLAYER MODE" };
            int x = 70, y = 140;

            for (int i = 0; i < butMenu.Length; i++)
            {
                Button temp = new Button();
                temp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                temp.Cursor = System.Windows.Forms.Cursors.Hand;
                temp.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                temp.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                temp.Name = "but" + i.ToString();
                temp.Size = new System.Drawing.Size(220, 50);
                temp.TabIndex = tabIndex; tabIndex++;
                temp.Text = btnText[i].ToString();
                temp.UseVisualStyleBackColor = false;
                temp.Click += butMenuHanders[i]; //assigning corresponding event handler to the button click event
                picBox_Menu.Controls.Add(temp);
                butMenu[i] = temp;
                
            }

            butMenu[0].Location = new System.Drawing.Point(x, y);
            butMenu[1].Location = new System.Drawing.Point(x, y+60);
            butMenu[2].Location = new System.Drawing.Point(x, y);
            butMenu[3].Location = new System.Drawing.Point(x, y + 60);
            butMenu[4].Location = new System.Drawing.Point(x, y + 120);
            butMenu[5].Location = new System.Drawing.Point(x, y + 60);
            butMenu[6].Location = new System.Drawing.Point(x, y + 120);

            // Hide "MULTIPLAYER", "PLAY WITH COMPUTER", "BACK TO START", "OK", "BACK TO PLAYER MODE" buttons,
            // only let "START", "QUIT" visible
            for (int i = 2; i < butMenu.Length; i++)
            {
                butMenu[i].Visible = false;
            }

        }
        // Start button - Click event to hide start and quit buttons
        private void btn_START_Click(object sender, EventArgs e)
        {
            butMenu[1].Visible = false;
            SlideInButtons();
            butMenu[0].Visible = false;
            
        }
        // when start button is click, "MULTIPLAYER", "PLAY WITH COMPUTER", "BACK TO START" buttons will be visible
        private void SlideInButtons()
        {
            Utils.Animate(butMenu[2], Utils.Effects.Roll, 100, 0);
            Utils.Animate(butMenu[3], Utils.Effects.Roll, 100, 0);
            Utils.Animate(butMenu[4], Utils.Effects.Roll, 100, 0);
        }
        // when the player choose one of the player mode to play, set score interface will be slided in
        private void SlideInSetScore()
        {
            Utils.Animate(lbl_numDice_Menu, Utils.Effects.Roll, 100, 0);
            Utils.Animate(txt_NumDice_Menu, Utils.Effects.Roll, 100, 0);
            Utils.Animate(butMenu[5], Utils.Effects.Roll, 100, 0);
            Utils.Animate(butMenu[6], Utils.Effects.Roll, 100, 0);
        }
        // click event on button quit to quit the application
        private void btn_QUIT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // when the player chooses mutiplayer mode, isMultiPlayer will be set to true to indicate that the player has choosen multiplayer mode
        private void btn_MULTIPLAYER_Click(object sender, EventArgs e)
        {
            isMultiPlayer = true;
            SlideInButtons();
            SlideInSetScore();
            
        }
        // when the player chooses play with computer mode
        // isPlayedWithComp will be set to true to indicate that the player has choosen play with computer mode
        private void btn_PLAY_COMPUTER_Click(object sender, EventArgs e)
        {
            isPlayedWithComp = true;
            Utils.Animate(lbl_PlayerDeterminant, Utils.Effects.Roll, 100, 0);
            SlideInButtons();
            SlideInSetScore();
        }
        // click event on btn_BACK_TO_START to set both play with computer and multplayer mode back to false
        // make start and quit buttons visible
        // slide out other buttons
        private void btn_BACK_TO_START_Click(object sender, EventArgs e)
        {
            
            SlideInButtons();
            isPlayedWithComp = false;
            isMultiPlayer = false;
            butMenu[0].Visible = true;
            butMenu[1].Visible = true;

        }

        // Ok button letting player start to player the game, if there are some error dialog windows will appear
        // to let the players know there the error are
        private void btn_OK_Click(object sender, EventArgs e)
        {
            string message, caption;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            // display the dialog window if the player did not enter any goal score
            if (txt_NumDice_Menu.Text.Length == 0)
            {
                message = "You did not enter any goal score, please provide one!";
                caption = "Undetermined goal score";
                
                result = MessageBox.Show(this, message, caption, buttons,
                MessageBoxIcon.Stop);
            }
            
            else {
                int temp = Convert.ToInt32(txt_NumDice_Menu.Text);
                // if the player has enter a goal score but not between 50 and 100, display dialog window
                // will appear showing the goal score is not valid
                if (temp < 50 || temp > 100)
                {
                    message = "Goal Score should be between 50 and 100!";
                    caption = "Set Goal Score";
                    result = MessageBox.Show(this, message, caption, buttons,
                    MessageBoxIcon.Stop);
                }
                // if goal score is valid then let the players start to play the game
                else
                {
                    but_RollDice.Enabled = true;
                    SlideInSetScore();
                    Utils.Animate(picBox_Menu, Utils.Effects.Roll, 100, 0);
                    Utils.Animate(dice_Table, Utils.Effects.Roll, 1000, 0);

                    iGoalScore = Convert.ToInt32(txt_NumDice_Menu.Text);
                    lbl_GoalScore_Game.Text = "Goal Score: " + txt_NumDice_Menu.Text;

                    turn = randTurn.Next(1, 2 + 1); //randomly choose player's turn
                    // by default isPlayedWithComp and isMultiPlayer boolean values are set to false
                    // isPlayedWithComp is set to true if the player choose to play with computer
                    // isMultiPlayer is set to true if the player choose to play with another player
                    if (isPlayedWithComp)
                    {
                        if (turn == 1)// Player1's turn
                        {
                            player1.GetPicRoll().Visible = true; //display an image that represent player1's turn
                            lblPlayerScore[1, 0].BackColor = Color.FromArgb(237, 26, 170); // set background color for the label on scoresheet
                        }
                        else //Player2's turn
                        {
                            player2.GetPicRoll().Visible = true; //display an image that represent player1's turn
                            lblPlayerScore[2, 0].BackColor = Color.FromArgb(237, 26, 170); // set background color for the label on scoresheet
                            Disable_radBut();
                            but_RollDice.Enabled = false;   //when the computer's turn, it will disable the roll button so the other player is unable to click the button
                            tmr_CompStart.Start(); // start timer tmr_CompStart which triggers click event on button btn_OK
                        }
                    }
                    else if (isMultiPlayer)
                    {
                        if (turn == 1)// Player1's turn
                        {
                            player1.GetPicRoll().Visible = true;    //display an image that represent player1's turn
                            lblPlayerScore[1, 0].BackColor = Color.FromArgb(237, 26, 170);
                        }
                        else //Player2's turn
                        {
                            player2.GetPicRoll().Visible = true;
                            lblPlayerScore[2, 0].BackColor = Color.FromArgb(237, 26, 170);
                        }
                    }
                }
            }
        }
        // this button is in the memu interface
        private void btn_BACK_TO_PLAYER_MODE_Click(object sender, EventArgs e)
        {
            lblPlayerScore[1, 0].BackColor = Color.Transparent;
            lblPlayerScore[2, 0].BackColor = Color.Transparent;
            txt_NumDice_Menu.Text = "50"; //set back to default goal score
            EraseScore(1);                  // erase score of player 1, this is something to do will labels on scoresheet
            EraseScore(2);                  // erase score of player 2
            PutScoreToSheet(player1, 1);    // put score to score to scoresheet, by default, the strings that represent score on dice will be set to empty
            PutScoreToSheet(player2, 2);
            player1.ResetPlayer();          // reset player1 subscore, total score, number of games won
            player2.ResetPlayer();          // reset player2 subscore, total score, number of games won
            radButChooseDice[5].Checked = true; //set back to chosen dice default
            isMultiPlayer = false; isPlayedWithComp = false;
            if (lbl_PlayerDeterminant.Visible) //if it is on playwith computer mode, lbl_PlayerDeterminant needs to set to invisible too
                Utils.Animate(lbl_PlayerDeterminant, Utils.Effects.Roll, 100, 0);
            SlideInSetScore(); //slide out the menu will setscore interface
            SlideInButtons();   //slide out the menu menu buttons
          
        }
        //Create the label which showing the message that player1 is always the human player
        private void CreatePlayerDeterminant()
        {
            this.lbl_PlayerDeterminant.BackColor = Color.FromArgb(33,144,181);
            this.lbl_PlayerDeterminant.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_PlayerDeterminant.ForeColor = System.Drawing.Color.Black;
            this.lbl_PlayerDeterminant.Location = new System.Drawing.Point(35, 75);
            this.lbl_PlayerDeterminant.Name = "lbl_numDice";
            this.lbl_PlayerDeterminant.Size = new System.Drawing.Size(290, 50);
            this.lbl_PlayerDeterminant.TabIndex = tabIndex; tabIndex++;
            this.lbl_PlayerDeterminant.Text = "Player 1 is always considered to be the human player";
            this.lbl_PlayerDeterminant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            picBox_Menu.Controls.Add(lbl_PlayerDeterminant);
            this.lbl_PlayerDeterminant.Visible = false;
        }
        // create label which asking the player to enter a goal score
        private void CreateChooseDiceField()
        {
            this.lbl_numDice_Menu.BackColor = System.Drawing.Color.Yellow;
            this.lbl_numDice_Menu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_numDice_Menu.ForeColor = System.Drawing.Color.Red;
            this.lbl_numDice_Menu.Location = new System.Drawing.Point(35, 148);
            this.lbl_numDice_Menu.Name = "lbl_numDice";
            this.lbl_numDice_Menu.Size = new System.Drawing.Size(160, 24);
            this.lbl_numDice_Menu.TabIndex = tabIndex; tabIndex++;
            this.lbl_numDice_Menu.Text = "SET GOAL SCORE:";
            this.lbl_numDice_Menu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            picBox_Menu.Controls.Add(lbl_numDice_Menu);
            this.lbl_numDice_Menu.Visible = false;
        }
        // create texbox where players can enter a goal score for their game
        private void CreateTxtDiceField()
        {
            this.txt_NumDice_Menu.Location = new System.Drawing.Point(200, 148);
            this.txt_NumDice_Menu.Name = "txt_NumDice";
            this.txt_NumDice_Menu.Text = iGoalScore.ToString();
            this.txt_NumDice_Menu.Size = new System.Drawing.Size(120, 24);
            this.txt_NumDice_Menu.TabIndex = tabIndex; tabIndex++;
            this.txt_NumDice_Menu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_NumDice_Menu_KeyPress);
            picBox_Menu.Controls.Add(txt_NumDice_Menu);
            this.txt_NumDice_Menu.Visible = false;
        }
        // this will make the player unable to press any key except digit and backspace keys on the keyboard
        // this will prevent the player when they try to enter characters that are not numbers
        private void txt_NumDice_Menu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8) //only acepts backspace and digit numbers
                e.Handled = true;
        }
        
        #endregion ENDS MENU INTERFACE

        #region PLAY INTERFACE
        // create a picture which represent the dice tabel, this picturebox is the parent contaning 6 radiobuttons (where players choose number of dice),
        // 6 picture boxes which show images that represent numbers on dice, scoresheet picturebox, number of dice choosen label on top right corner,
        // goal score label on top left corner and a roll dice button
        private void CreateTable()
        {
            dice_Table.BackColor = System.Drawing.Color.Transparent;
            //background image has sourced from: http://www.outrageousfortunes.co.uk/?attachment_id=1116
            dice_Table.Image = VanGiangPhamAssgt.Properties.Resources.table;
            dice_Table.Location = new System.Drawing.Point(0,0);
            dice_Table.Name = "picBox_Table";
            dice_Table.Size = new System.Drawing.Size(994, 621);
            dice_Table.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            dice_Table.TabIndex = tabIndex; tabIndex++;
            dice_Table.TabStop = false;
            dice_Table.Visible = false;
            this.Controls.Add(dice_Table);
        }
        // this picturebox will be the parent of all labels that display players' results of each roll
        private void CreatePicScoreSheet()
        {
            picBox_ScoreSheet.BackColor = System.Drawing.SystemColors.Control;
            picBox_ScoreSheet.Location = new System.Drawing.Point(64, 146);
            picBox_ScoreSheet.Name = "picBox_Score";
            picBox_ScoreSheet.Size = new System.Drawing.Size(238, 353);
            picBox_ScoreSheet.TabIndex = tabIndex; tabIndex++;
            picBox_ScoreSheet.TabStop = false;
            dice_Table.Controls.Add(picBox_ScoreSheet);
        }
        // this is to draw line on picBox_ScoreSheet, fill color and create labels which display players' results of each roll
        private void DrawScoreSheet()
        {
            // if we draw lines on picturebox, it will draw but in a few milisecons, lines will disappear,
            // so will draw on a bitmap image then set the bitmap image to the picturebox's image property
            Bitmap DrawLines;
            DrawLines = new Bitmap(picBox_ScoreSheet.Size.Width, picBox_ScoreSheet.Size.Height);
            Graphics g = Graphics.FromImage(DrawLines);
            Pen blackPen = new Pen(Brushes.Black, 1);
            Font myFont = new Font("Arial", 12);
            Color cusColor = Color.FromArgb(233, 249, 12);
            int ver_lines = 4;
            int hor_lines = 10;
            float x = 0f, y = 0f;
            float xSpace = picBox_ScoreSheet.Width / (ver_lines - 1);
            float ySpace = picBox_ScoreSheet.Height / (hor_lines - 1);
            string[,] props = new String[3, 9]{
                {"Die", "1st", "2nd", "3rd","4th", "5th", "6th", "Sub-total:", "Total:"},
                {"Player 1", "", "", "", "", "", "", "0", "0"},
                {"Player 2", "", "", "", "", "", "", "0", "0"}
            };
            
            Rectangle rectTop = new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(picBox_ScoreSheet.Width), Convert.ToInt32(ySpace));
            g.FillRectangle(new SolidBrush(cusColor), rectTop);

            Rectangle rectBot = new Rectangle(Convert.ToInt32(x), Convert.ToInt32(picBox_ScoreSheet.Height - 2 * ySpace), Convert.ToInt32(picBox_ScoreSheet.Width), Convert.ToInt32(picBox_ScoreSheet.Height));
            g.FillRectangle(new SolidBrush(cusColor), rectBot);

            
            for (int i = 0; i < ver_lines; i++)
            {
                g.DrawLine(blackPen, x, y, x, picBox_ScoreSheet.Height);
                x += xSpace;
            }
            x = 0f;
            for (int i = 0; i < hor_lines; i++)
            {
                g.DrawLine(blackPen, x, y, picBox_ScoreSheet.Width, y);
                y += ySpace;
            }
            picBox_ScoreSheet.Image = DrawLines;
            g.Dispose();
            
            x = 0f;
            y = 0f;
            int labelNum = 1;
            //after drawing line on the picturebox, we then create labels to display information of each player
            
            for (int col = 0; col < ver_lines - 1; col++)
            {
                for (int row = 0; row < hor_lines - 1; row++)
                {
                    Label temp = new Label();
                    temp.AutoSize = false;
                    //temp.BackColor = Color.FromArgb(237,26,170);
                    temp.BackColor = Color.Transparent;
                    temp.Location = new Point(Convert.ToInt32(xSpace) / 15 + Convert.ToInt32(x), Convert.ToInt32(ySpace) / 4 + Convert.ToInt32(y));
                    temp.Name = "lbl_Score" + String.Format("{0:2d}", labelNum);
                    temp.Size = new System.Drawing.Size(70, 21);
                    temp.TabIndex = tabIndex; tabIndex++;
                    temp.Text = Convert.ToString(props[col, row]);
                    temp.TextAlign = ContentAlignment.MiddleCenter;
                    picBox_ScoreSheet.Controls.Add(temp);
                    lblPlayerScore[col, row] = temp;
                    labelNum++;
                    y += ySpace;
                }
                x += xSpace;
                y = 0;
            }
        } 
        // create pictureboxes to display dice images, there 6 pictureboxes to display dice results of each roll
        // the image property of these pictureboxes will be set to none depending on the number of dice the players
        // choose to roll. By default, those images that represent dice from iDiceDef array will be set to 
        // these pictureboxes' image property
        private void CreateDicePictureBoxes()
        {
            int x = 450, y = 400;

            for (int i = 0; i < picBoxImages.Length; i++)
            {
                PictureBox temp = new PictureBox();
                temp.BackColor = Color.FromArgb(50, Color.White);
                temp.Location = new System.Drawing.Point(x, y);
                temp.Name = "picBox_Dice" + i.ToString();
                temp.Size = new System.Drawing.Size(50, 50);
                temp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                temp.TabIndex = tabIndex; tabIndex++;
                temp.TabStop = false;
                dice_Table.Controls.Add(temp);
                picBoxImages[i] = temp;
                x += 60;
            }
        }
        // this method shows those pictureboxes that reprent dice when the player chooses number of dice to roll
        // for example, if they choose 4 dice to roll, the only 4 dice images will be shown
        private void DisplayPicImgs(int numImgs)
        {
            for (int i = 0; i < numImgs; i++)
            {
                picBoxImages[i].Image = diceImages[iDicDef[i] - 1];
            }
        }
        // method to hide images that reperent dice, this method will be called at the same time as DisplayPicImgs(int numImgs) method
        // once again, they will be hided when the player chooses how many dice to roll, for example, if they choose 4 dice to roll,
        // first 4 dice will be shown and the last 2 dice will be hided.
        private void HideImages(int numImgs)
        {
            for (int i = picBoxImages.Length - 1; i > numImgs - 1; i--)
            {
                if (picBoxImages[i].Image != null)
                    picBoxImages[i].Image = null;
            }

        }
        // this method assigns the event handler of each radioButton's event to the radHandler array
        private void RadioButHandler()
        {
            radHandler[0] = this.radDice1_CheckedChanged;
            radHandler[1] = this.radDice2_CheckedChanged;
            radHandler[2] = this.radDice3_CheckedChanged;
            radHandler[3] = this.radDice4_CheckedChanged;
            radHandler[4] = this.radDice5_CheckedChanged;
            radHandler[5] = this.radDice6_CheckedChanged;
        }
        // create radio buttons, these button represent the number of dice the player wants to roll
        // be default, radiobutton that represents 6 dice will be set as "checked"
        private void CreateRadButtons()
        {
            string[] radText = new String[6] { "1 Die", "2 Dice", "3 Dice", "4 Dice", "5 Dice", "6 Dice" };
            int x = 890, y = 150;
            for (int i = 0; i < radButChooseDice.Length; i++)
            {
                RadioButton temp = new RadioButton();
                temp.Appearance = System.Windows.Forms.Appearance.Button;
                temp.AutoSize = false;
                temp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                temp.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(27,150,191);
                temp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                temp.Cursor = System.Windows.Forms.Cursors.Hand;
                temp.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                temp.Location = new System.Drawing.Point(x, y);
                temp.Name = "chooseDice6";
                temp.Size = new System.Drawing.Size(60, 50);
                temp.TabIndex = tabIndex; tabIndex++;
                temp.TabStop = true;
                temp.Text = radText[i];
                temp.UseVisualStyleBackColor = false;
                temp.CheckedChanged += radHandler[i];
                dice_Table.Controls.Add(temp);
                radButChooseDice[i] = temp;
                y += 50;
            }
            radButChooseDice[5].Checked = true;
        }
        // 1 Die radio button event handler
        private void radDice1_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 1";
            iNumOfDice = 1;
            DisplayPicImgs(iNumOfDice);
            HideImages(iNumOfDice);

        }
        // 1 Dice radio button event handler
        private void radDice2_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 2";
            iNumOfDice = 2;
            DisplayPicImgs(iNumOfDice);
            HideImages(iNumOfDice);

        }
        // 3 Dice radio button event handler
        private void radDice3_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 3";
            iNumOfDice = 3;
            DisplayPicImgs(iNumOfDice);
            HideImages(iNumOfDice);

        }
        // 4 Dice radio button event handler
        private void radDice4_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 4";
            iNumOfDice = 4;
            DisplayPicImgs(iNumOfDice);
            HideImages(iNumOfDice);

        }
        // 5 Dice radio button event handler
        private void radDice5_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 5";
            iNumOfDice = 5;
            DisplayPicImgs(iNumOfDice);
            HideImages(iNumOfDice);
            
        }
        // 6 Dice radio button event handler
        private void radDice6_CheckedChanged(object sender, EventArgs e)
        {
            lbl_NumDice_Game.Text = "Num of Dice: 6";
            iNumOfDice = 6;
            DisplayPicImgs(iNumOfDice);
                
        }
       // create goal score label of the game play view
        private void CreateGoalScoreGame()
        {
            this.lbl_GoalScore_Game.BackColor = System.Drawing.Color.Yellow;
            this.lbl_GoalScore_Game.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_GoalScore_Game.ForeColor = System.Drawing.Color.Red;
            this.lbl_GoalScore_Game.Location = new System.Drawing.Point(43, 8);
            this.lbl_GoalScore_Game.Name = "lbl_GoalScore";
            this.lbl_GoalScore_Game.Size = new System.Drawing.Size(149, 33);
            this.lbl_GoalScore_Game.TabIndex = tabIndex; tabIndex++;
            this.lbl_GoalScore_Game.Text = "Goal Score:";
            this.lbl_GoalScore_Game.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            dice_Table.Controls.Add(lbl_GoalScore_Game);

        }
        // create number of dice choosen label of the game play view
        private void CreateNumOfDiceGame()
        {
            lbl_NumDice_Game.BackColor = System.Drawing.Color.Yellow;
            lbl_NumDice_Game.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lbl_NumDice_Game.ForeColor = System.Drawing.Color.Red;
            lbl_NumDice_Game.Location = new System.Drawing.Point(800, 8);
            lbl_NumDice_Game.Name = "lbl_ChooseDice";
            lbl_NumDice_Game.Size = new System.Drawing.Size(149, 33);
            lbl_NumDice_Game.TabIndex = tabIndex; tabIndex++;
            lbl_NumDice_Game.Text = "Num of Dice:";
            lbl_NumDice_Game.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            dice_Table.Controls.Add(lbl_NumDice_Game);
        }
        // create pictureboxes that reprent players'turn and players' winning
        private void CreatePicPRoll(PictureBox picBox)
        {
            if (picBox.Equals(picBox_P1Roll)) //picturebox that reprents player1'turn
            {
                picBox.Image = global::VanGiangPhamAssgt.Properties.Resources.player1Roll;
                picBox.Name = "picBox_P1Roll";
                picBox.Location = new System.Drawing.Point(462, 54);
                dice_Table.Controls.Add(picBox);
            }
            else if (picBox.Equals(picBox_P2Roll)) //picturebox that reprents player2'turn
            {  //picBox is picBox_P2Roll
                picBox.Image = global::VanGiangPhamAssgt.Properties.Resources.player2Roll;
                picBox.Name = "picBox_P2Roll";
                picBox.Location = new System.Drawing.Point(462, 54);
                dice_Table.Controls.Add(picBox);
            }
            else if (picBox.Equals(picBox_P1Win)) //picturebox that reprents player1'winning
            {
                picBox.Image = global::VanGiangPhamAssgt.Properties.Resources.player1Wins;
                picBox.Name = "player1Wins";
                picBox.Location = new System.Drawing.Point(362, 104);
                picBox_Bg.Controls.Add(picBox);
                
            }
            else if (picBox.Equals(picBox_P2Win)) //picturebox that reprents player2'winning
            {
                picBox.Image = global::VanGiangPhamAssgt.Properties.Resources.play2Wins;
                picBox.Name = "player2Wins";
                picBox.Location = new System.Drawing.Point(362, 104);
                picBox_Bg.Controls.Add(picBox);
            
            }
           
            picBox.Size = new System.Drawing.Size(285, 180);
            picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picBox.TabIndex = tabIndex; tabIndex++;
            picBox.TabStop = false;
            picBox.Visible = false;
            
        }
        // create roll dice button
        private void CreateRollButton()
        {
            but_RollDice.Cursor = System.Windows.Forms.Cursors.Hand;
            but_RollDice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            but_RollDice.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            but_RollDice.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            but_RollDice.Location = new System.Drawing.Point(560, 497);
            but_RollDice.Name = "btn_RollDice";
            but_RollDice.Size = new System.Drawing.Size(150, 50);
            but_RollDice.TabIndex = tabIndex; tabIndex++;
            but_RollDice.Text = "ROLL DICE";
            but_RollDice.UseVisualStyleBackColor = false;
            but_RollDice.Click += new System.EventHandler(but_RollDice_Click);
            dice_Table.Controls.Add(but_RollDice);

        }

        #endregion ENDS PLAY INTERFACE

        #region GAME RESULTS INTERFACE
        // create picturebox for the winning screen
        private void CreateWinningScreen()
        {

            picBox_Bg.BackColor = System.Drawing.Color.Transparent;
            picBox_Bg.Location = new System.Drawing.Point(0, 0);
            picBox_Bg.Name = "picBox_Table";
            picBox_Bg.Size = new System.Drawing.Size(994, 621);
            picBox_Bg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picBox_Bg.TabIndex = tabIndex; tabIndex++;
            picBox_Bg.TabStop = false;
            picBox_Bg.Visible = false;
            this.Controls.Add(picBox_Bg);

            Bitmap Fill;
            Color cTop = Color.FromArgb(125, 205, 232);
            Color cBottom = Color.FromArgb(0, 88, 117);

            Fill = new Bitmap(picBox_Bg.Size.Width, picBox_Bg.Size.Height);
            Graphics g = Graphics.FromImage(Fill);
            LinearGradientBrush brush = new LinearGradientBrush(new PointF(0, 0), new Point(0, Fill.Height),
                cTop, cBottom);
            g.FillRectangle(brush, new RectangleF(0, 0, Fill.Width, Fill.Height));
            picBox_Bg.Image = Fill;
            g.Dispose();

        }
        // This is to display whether the player got boojum, dead drop, snake's eyes, snaffle, one 1 or other case
        private void CreatPicResultsBan() 
        {
            picBox_Banner.BackColor = System.Drawing.Color.FromArgb(80, 0, 0, 0);
            picBox_Banner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            picBox_Banner.Location = new System.Drawing.Point(505, -70);
            picBox_Banner.Name = "pictureBox1";
            picBox_Banner.Size = new System.Drawing.Size(200, 70);
            picBox_Banner.TabIndex = 1;
            picBox_Banner.TabStop = false;
            dice_Table.Controls.Add(picBox_Banner);
            picBox_Banner.BringToFront();
        }
        //Create a label so that we can easily display the results of each roll on the results banner created above
        private void CreateBanLabel()
        {
            ban_Label.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ban_Label.ForeColor = System.Drawing.Color.White;
            ban_Label.Location = new System.Drawing.Point(2, 2);
            ban_Label.Name = "label1";
            ban_Label.Size = new System.Drawing.Size(200, 60);
            ban_Label.TabIndex = 1;
            ban_Label.Text = "GAME STARTS";
            ban_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            picBox_Banner.Controls.Add(ban_Label);
        }
        //create label that reprents number of games won by each player
        private void Create_Llb_GamesWon(Label lbl)
        {
            if (lbl.Equals(lbl_GamesWon))
            {
                lbl.Location = new System.Drawing.Point(350, 330);
                lbl.Name = "lbl_P1GameWon";
                lbl.Text = "Games Won\n";
            }
            else if (lbl.Equals(lbl_P1GamesWon))
            {
                lbl.Location = new System.Drawing.Point(460, 330);
                lbl.Name = "lbl_P2GameWon";
            }
            else if (lbl.Equals(lbl_P2GamesWon))
            {
                lbl.Location = new System.Drawing.Point(560, 330);
                lbl.Name = "lbl_P2GameWon";
            }
            lbl.Font = new System.Drawing.Font("Impact", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl.ForeColor = System.Drawing.Color.Black;
            lbl.Size = new System.Drawing.Size(100, 60);
            lbl.TabIndex = tabIndex; tabIndex++;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            picBox_Bg.Controls.Add(lbl);
        }
        // create play again and back to player mode buttons, these buttons will only be shown on the winning screen
        private void Create_PlayAgain_BackMode(Button btn)
        {
            if (btn.Equals(btn_PlayAgain))
            {
                btn.Location = new Point(433, 424);
                btn.Text = "PLAY AGAIN";
                btn.Click += this.btnPlayAgain_Click;
            }
            else
            {
                btn.Location = new Point(433, 484);
                btn.Text = "BACK TO PLAYER MODE";
                btn.Click += this.btnBackMode_Click;
            }
            btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn.Size = new System.Drawing.Size(150, 50);
            btn.TabIndex = tabIndex; tabIndex++;
            btn.UseVisualStyleBackColor = false;
            picBox_Bg.Controls.Add(btn);
            btn.Visible = false;
        }
        // btnPlayAgain click event
        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            if (picBox_P1Win.Visible) //if picBox_P1Win is visible then make it invisible
                Utils.Animate(picBox_P1Win, Utils.Effects.Roll, 100, 45);

            if (picBox_P2Win.Visible) //if picBox_P1Win is visible then make it invisible
                Utils.Animate(picBox_P2Win, Utils.Effects.Roll, 100, 45);
            player1.ResetResults(); //reset results: subscore, total score
            player2.ResetResults();
            lblPlayerScore[1, 0].BackColor = Color.Transparent;
            lblPlayerScore[2, 0].BackColor = Color.Transparent;
            txt_NumDice_Menu.Text = "50"; //set back to default
            EraseScore(1); // this will set texts of the labels that display number of on dice to empty strings
            EraseScore(2);
            PutScoreToSheet(player1, 1); // this will set texts of the labels that display sub score and total score to 0 strings
            PutScoreToSheet(player2, 2); // this will set texts of the labels that display sub score and total score to 0 strings
            radButChooseDice[5].Checked = true; //set back to chosen dice default

            //slide out winning screen screen interface
            Utils.Animate(btn_BackToPlayerMode, Utils.Effects.Roll, 100, 45);
            Utils.Animate(btn_PlayAgain, Utils.Effects.Roll, 100, 45);
            Utils.Animate(picBox_Bg, Utils.Effects.Roll, 100, 45);
            //slide in menu buttons and scoreset interface
            Utils.Animate(picBox_Menu, Utils.Effects.Roll, 100, 0);
            SlideInSetScore();

        }
        //black to player mode button - click event
        private void btnBackMode_Click(object sender, EventArgs e)
        {
            isPlayedWithComp = false;
            isMultiPlayer = false;
            if (picBox_P1Win.Visible)
                Utils.Animate(picBox_P1Win, Utils.Effects.Roll, 100, 45);
            if (picBox_P2Win.Visible)
                Utils.Animate(picBox_P2Win, Utils.Effects.Roll, 100, 45);

            lblPlayerScore[1, 0].BackColor = Color.Transparent;
            lblPlayerScore[2, 0].BackColor = Color.Transparent;
            txt_NumDice_Menu.Text = "50"; //set back to default
            EraseScore(1);
            EraseScore(2);
            PutScoreToSheet(player1, 1);
            PutScoreToSheet(player2, 2);
            player1.ResetPlayer(); //reset player: subscore, total score, number of games won
            player2.ResetPlayer();
            radButChooseDice[5].Checked = true; //set back to chosen dice default
            Utils.Animate(btn_PlayAgain, Utils.Effects.Roll, 100, 45);
            Utils.Animate(btn_BackToPlayerMode, Utils.Effects.Roll, 100, 45);
            Utils.Animate(picBox_Bg, Utils.Effects.Roll, 100, 45);
            Utils.Animate(picBox_Menu, Utils.Effects.Roll, 100, 0);
            SlideInButtons();
            
        }
        
        private void GameResultsInterface()
        {
            //slide out the game player interface when a player wins the game
            Utils.Animate(dice_Table, Utils.Effects.Slide, 1000, 0);
            Utils.Animate(picBox_Bg, Utils.Effects.Slide, 100, 0);
            //make buttons in the winning screen visible
            btn_PlayAgain.Visible = true;
            btn_BackToPlayerMode.Visible = true;
        }
        #endregion

        #region GAME PLAY IMPLEMENTATION
        private void Disable_radBut()
        {
            for (int i = 0; i < radButChooseDice.Length; i++)
                radButChooseDice[i].Enabled = false;
        }
        
        private void but_RollDice_Click(object sender, EventArgs e)
        {
            if (isMultiPlayer) //play against another player
            {
                if (turn == 1) //player1'turn
                {
                    Utils.Animate(player1.GetPicRoll(), Utils.Effects.Roll, 100, 0); // display image that reprents player1's turn
                    EraseScore(2); //Erease player2's score
                    //set number of dice to the dice array of player1, this will be the size of the dice array
                    player1.SetDiceArray(iNumOfDice);
                    //call RollDice method below
                    RollDice(player1, 1);
                    //call GetResults method below
                    GetResults(player1, player2, 1);
                    //after roll dice and get dice results are done, we reset dice results of player1
                    player1.ResetDiceResults();

                    // give player2 the turn
                    turn = 2;
                }
                else
                {
                    Utils.Animate(player2.GetPicRoll(), Utils.Effects.Roll, 100, 0);
                    EraseScore(1); //Erease player1's score

                    player2.SetDiceArray(iNumOfDice);
                    RollDice(player2, 2);
                    GetResults(player2, player1, 2);
                    player2.ResetDiceResults();

                    turn = 1;
                }
            }

            if (isPlayedWithComp) //play against computer
            {
                if (turn == 1)
                {

                    Utils.Animate(player1.GetPicRoll(), Utils.Effects.Roll, 100, 0); // display image that reprents player1's turn
                    EraseScore(2); //Erease player2's score 
                    //set number of dice to the dice array of player1, this will be the size of the dice array
                    player1.SetDiceArray(iNumOfDice);
                    //call RollDice method below
                    RollDice(player1, 1);
                    //call GetResults method below
                    GetResults(player1, player2, 1);
                    //PutScoreToSheet(player1, 1);
                    
                    if ((player1.GetDiceResultsElement(0)) <3 && (player1.GetTotalScore() < iGoalScore)) //player1 does not win or loose yet  
                    {
                        player1.ResetDiceResults();
                        turn = 2;
                        but_RollDice.Enabled = false;
                        tmr_CompStart.Start();
                    }            
                }
                else
                {
                    Utils.Animate(player2.GetPicRoll(), Utils.Effects.Roll, 100, 0);
                    EraseScore(1); //Erease player1's score
                    CompChooseDice(); //method letting computer choose number of dice to roll based on its component's results
                    player2.SetDiceArray(iNumOfDice);
                    RollDice(player2, 2);
                    GetResults(player2, player1, 2);
                    player2.ResetDiceResults();

                    turn = 1;

                }
             
            }
            
        }
        // this method will let the computer choose how many dice to roll based on some conditions
        private void CompChooseDice()
        {
            double dGoalScorePercent = 100.0; // convert preset total goal score to percentage
            // convert the current goal score computer has to percentage as well
            double dTotalScorePercent = (player2.GetTotalScore() * 100) / iGoalScore;
            
            if ((player2.GetTotalScore() <= player1.GetTotalScore())) //if goals score of computer is <= goal score of human player
            {
                //AND...
                if (dTotalScorePercent <= dGoalScorePercent / 2) //half of preset total score
                    radButChooseDice[5].Checked = true; //choose 6 dice to roll
                else if (player2.GetTotalScore() <= (2 * dGoalScorePercent) / 3) //two third of preset total score
                    radButChooseDice[4].Checked = true; //choose 5 dice to roll
                else if (player2.GetTotalScore() <= (3 * dGoalScorePercent) / 4) //3 fourth of preset total score
                    radButChooseDice[3].Checked = true; //choose 4 dice to roll
                else if (player2.GetTotalScore() <= (4 * dGoalScorePercent) / 5) //4 fifth of preset total score
                    radButChooseDice[2].Checked = true; //choose 3 dice to roll
                else if (player2.GetTotalScore() <= (5 * dGoalScorePercent) / 6) //5 sixth of preset total score
                    radButChooseDice[1].Checked = true; //choose 2 dice to roll
                else
                    radButChooseDice[0].Checked = true; //choose 1 dice to roll
            }
            else //if goals score of computer is > goal score of human player
            {
                if (dTotalScorePercent <= dGoalScorePercent / 2) //half of preset total score
                    radButChooseDice[4].Checked = true; //choose 5 dice to roll
                else if (player2.GetTotalScore() <= (2 * dGoalScorePercent) / 3) //two third of preset total score
                    radButChooseDice[3].Checked = true; //choose 4 dice to roll
                else if (player2.GetTotalScore() <= (3 * dGoalScorePercent) / 4) //3 fourth of preset total score
                    radButChooseDice[2].Checked = true; //choose 3 dice to roll
                else if (player2.GetTotalScore() <= (4 * dGoalScorePercent) / 5) //4 fifth of preset total score
                    radButChooseDice[1].Checked = true; //choose 2 dice to roll
                else
                    radButChooseDice[0].Checked = true; //choose 1 dice to roll
            }
        }   
           
        private void RollDice(Player p, int position) //position = 1 means player 1, position = 2, player 2
        {
            int randTemp;
            for (int i = 0; i < p.GetDice().Length; i++)
            {
                randTemp = randRoll.Next(1, 6 + 1);
                p.NumOnDice(i, randTemp);           //set score on each die to the dice array of a player
                picBoxImages[i].Image = diceImages[randTemp - 1]; //set image property for a picturebox, this image reprent number on dice
                picBoxImages[i].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                lblPlayerScore[position, i + 1].Text = p.GetDiceElement(i).ToString(); //put scores on dice to coresponding player
                
                switch (p.GetDiceElement(i))
                {
                    case 1:
                        p.SetDiceResults(0); //increments the number of 1s that has been rolled
                        break;
                    case 2:
                        p.SetDiceResults(1); //increments the number of 2s that has been rolled
                        break;
                    case 3:
                        p.SetDiceResults(2); //increments the number of 3s that has beem rolled
                        break;
                    case 4:
                        p.SetDiceResults(3); //increments the number of 4s that has beem rolled
                        break;
                    case 5:
                        p.SetDiceResults(4); //increments the number of 5s that has beem rolled
                        break;
                    case 6:
                       p.SetDiceResults(5); //increments the number of 6s that has beem rolled
                        break;
                }
            }
        }

        private void GetResults(Player thisPlayer, Player thatPlayer, int position)
        //thisPlayer is not necessary player1, so as thatPlayer for player2
        //it could be another way around depending on the player's turn
        {
            //In p.GetDiceResults array:
            //GetDiceResultsElement(0) = number of 1s, GetDiceResultsElement(1) = number of 2s, GetDiceResultsElement(2) = number of 3s
            //GetDiceResultsElement(3) = number of 4s, GetDiceResultsElement(4) = number of 5s, GetDiceResultsElement(5) = number of 6s
            if (thisPlayer.GetDiceResultsElement(0) >= 4) 
                // Has Boojum - wins the game immediately
         
            {
                but_RollDice.Enabled = false; // disable roll button so the other player unable to click
                thisPlayer.SetGamesWon();

                ban_Label.Text = "BOOJUM!\nGAME OVER";
                tmr_SlideDownUp.Start();
                
                lbl_P1GamesWon.Text = "Player 1\n" + player1.GetGamesWon().ToString();
                lbl_P2GamesWon.Text = "Player 2\n" + player2.GetGamesWon().ToString();
                thisPlayer.GetPicWin().Visible = true;
               
                tmr_ShowResults.Start(); // timer which triggers showing winning interface
            }
            else if (thisPlayer.GetDiceResultsElement(0) == 3)
            {
                //Has Dead Drop - loses the game immediately
                but_RollDice.Enabled = false;   // disable roll button so the other player unable to click
                thatPlayer.SetGamesWon();
                ban_Label.Text = "DEAD DROP!\nGAME OVER";
                tmr_SlideDownUp.Start();

                lbl_P1GamesWon.Text = "Player 1\n" + player1.GetGamesWon().ToString();
                lbl_P2GamesWon.Text = "Player 2\n" + player2.GetGamesWon().ToString();
                thatPlayer.GetPicWin().Visible = true;
                
                tmr_ShowResults.Start();
            }
            else if (thisPlayer.GetDiceResultsElement(0) == 2) //Has Snake's eyes - scores nothing and total score is reset to zero
            {
                tmr_DisableBut.Start(); //timer which enables roll button in a certain time
                Disable_radBut();
                but_RollDice.Enabled = false;
                thisPlayer.SetSubScore(0);
                thisPlayer.ReSetTotalScore();
                PutScoreToSheet(thisPlayer, position);

                ban_Label.Text = "SNAKE'S EYES!" +"\nScore back to 0";

                tmr_SlideDownUp.Start(); //slide down and up banner if a player has snake's eyes
                tmr_PlayerTurn.Start();
            }
            else if (thisPlayer.GetDiceResultsElement(0) == 1)  //Has one 1 -  scores nothing for the turn
            {
                tmr_DisableBut.Start();
                Disable_radBut();
                but_RollDice.Enabled = false;
               
                thisPlayer.SetSubScore(0);

                PutScoreToSheet(thisPlayer, position);

                ban_Label.Text = "GOT ONE 1!" + "\nNo score";
                tmr_SlideDownUp.Start();
                tmr_PlayerTurn.Start();
            }
            else
            {
                bool flag = false;
                for (int i = 1; i < thisPlayer.GetDiceResults().Length; i++)
                {
                    //Has Snaffle - Score for that turn is twice the sum of the numbers on the dice that were rolled
                    if (thisPlayer.GetDiceResultsElement(i) >= 3)
                        flag = true;
                }
                if (flag)
                {
                    int temp = 0;
                    for (int j = 0; j < thisPlayer.GetDice().Length; j++)
                        temp += thisPlayer.GetDiceElement(j);
                    temp = temp * 2;
                    
                    thisPlayer.SetSubScore(temp);
                    thisPlayer.SetTotalScore();

                    PutScoreToSheet(thisPlayer, position);

                    ban_Label.Text = "SNAFFLE!" + "\nDouble score";
                    tmr_SlideDownUp.Start();
                }
                else
                {
                    int temp = 0;
                    for (int j = 0; j < thisPlayer.GetDice().Length; j++)
                        temp += thisPlayer.GetDiceElement(j);
                    
                    thisPlayer.SetSubScore(temp);
                    thisPlayer.SetTotalScore();
                    PutScoreToSheet(thisPlayer, position);
                }

                if (thisPlayer.GetTotalScore() >= iGoalScore)
                {
                    but_RollDice.Enabled = false;
                    thisPlayer.SetGamesWon();

                    ban_Label.Text = "SCORE REACHED!" + "\nGAME OVER";
                    tmr_SlideDownUp.Start();

                    lbl_P1GamesWon.Text = "Player 1\n" + player1.GetGamesWon().ToString();
                    lbl_P2GamesWon.Text = "Player 2\n" + player2.GetGamesWon().ToString();
                    thisPlayer.GetPicWin().Visible = true;
                    tmr_ShowResults.Start();
                }
                else {
                    tmr_DisableBut.Start();
                    Disable_radBut();
                    but_RollDice.Enabled = false;
                    tmr_PlayerTurn.Start();
                }
            }
        }

        private void PutScoreToSheet(Player p, int position)
        {
            lblPlayerScore[position, lblPlayerScore.GetLength(1) - 1].Text = p.GetTotalScore().ToString();
            lblPlayerScore[position, lblPlayerScore.GetLength(1) - 2].Text = p.GetSubScore().ToString();
        }

        private void EraseScore(int position) //position = 0 means dice number, position = 1 means player 1 score, positon = 2 means player 2 score
        {
            for (int i = 1; i < lblPlayerScore.GetLength(1) - 2; i++) //GetLength(1) means get length of the second dimensional array.
                lblPlayerScore[position, i].Text = ""; //set each score field to an empty string.
        }
       
        #endregion ENDS GAME PLAY IMPLEMENTATION

        #region TIMER EVENTS
        int duration = 0;
        // timer event which lets the picbox banner slide down and up
        private void tmr_SlideDownUp_Tick(object sender, EventArgs e)
        {
            //interval : 10 miliseconds, every tick picBox_Banner travels 110 pixels
            //Slide down
            //3 ticks picBox_Banner travels down 330 pixels
            if (duration < 3)
            {

                picBox_Banner.Top += 110;
                duration++;
            }
            //not moving for 600 miliseconds
            else if (duration < 63)
            {
                duration++;
            }
            // slide up
            //3 ticks picBox_Banner travels up 330 pixels
            else if (duration < 66)
            {
                picBox_Banner.Top -= 110; ;
                duration++;
            }

            else
            {
                tmr_SlideDownUp.Stop(); //stop timer
                duration = 0; //set duration to 0 for the next call
            }
        }

        // timer event which shows the results of the players' winning
        private void tmr_ShowResults_Tick(object sender, EventArgs e)
        {
            //interval: 3000 miliseconds
            if (duration < 1)
            {
                GameResultsInterface();
                duration++;
            }
            else
            {
                tmr_ShowResults.Stop();
                duration = 0; //set duration to 0 for the next call
            }

        }
        // timer event which enable the roll dice button and radion buttons
        private void tmr_DisableBut_Tick(object sender, EventArgs e)
        {
            but_RollDice.Enabled = true;
            for (int i = 0; i < radButChooseDice.Length; i++)
                radButChooseDice[i].Enabled = true; 
            tmr_DisableBut.Stop();
        }
        // swapping background color of labels that reprensent players' turn
        private void tmr_PlayerTurn_Tick(object sender, EventArgs e)
        {
            if (turn == 1) // if player1's turn
            {
                lblPlayerScore[2, 0].BackColor = Color.Transparent;
                picBox_P1Roll.Visible = true;
                lblPlayerScore[1, 0].BackColor = Color.FromArgb(237, 26, 170);
            }
            else if (turn == 2) // if player2's turn
            {
                picBox_P2Roll.Visible = true;
                lblPlayerScore[1, 0].BackColor = Color.Transparent;
                lblPlayerScore[2, 0].BackColor = Color.FromArgb(237, 26, 170);
            }

            tmr_PlayerTurn.Stop();
        }
        //timer which enabels roll button and performs its its click event
        private void tmr_CompStart_Tick(object sender, EventArgs e)
        {
            but_RollDice.Enabled = true;
            but_RollDice.PerformClick();
            tmr_CompStart.Stop();
        }
        #endregion
       
    }
}
