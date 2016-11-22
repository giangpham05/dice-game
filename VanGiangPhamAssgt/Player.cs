#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace VanGiangPhamAssgt
{
    public class Player
    {
        #region Decleration
        private int[] iDice;                        //store numbers on dice
        private int[] iDiceResults;                 //store occurence of 1s, 2s, 3s,4s, 5s and 6s
        private int iGamesWon;                      //store number of games won by player
        private int iSubScore, iTotalScore;         //store subscore, total score
        private PictureBox picBoxRoll, picBoxWin;   //store picBoxRoll(player's turn indication), picBoxWin(player's winning)
        #endregion

        #region Initialization
        //Constructor
        public Player(PictureBox picRoll, PictureBox picWin)
        {

            iDiceResults = new int[6] { 0, 0, 0, 0, 0, 0 };
            iGamesWon = 0;
            iSubScore = 0;
            iTotalScore = 0;
            picBoxRoll = picRoll;
            picBoxWin = picWin;

        }
        
        #endregion

        #region Properties
        // set size for dice array, intialize default values
        public void SetDiceArray(int iNumOfDice)
        {
            iDice = new int[iNumOfDice];
            for (int i = 0; i < iDice.Length; i++)
                iDice[i] = 0;
        }
        //set number on a die to a specific die element
        public void NumOnDice(int iPosition, int iRandResult)
        {
            iDice[iPosition] = iRandResult;
        }
        // get the picBoxRoll control
        public PictureBox GetPicRoll()
        {
            return picBoxRoll;
        }
        // get the picBoxWin control
        public PictureBox GetPicWin()
        {
            return picBoxWin;
        }
       

        //Get dice array
        public int[] GetDice()
        {
            return iDice;
        }
        //get a specific dice element
        public int GetDiceElement(int iDicePosition)
        {
            return iDice[iDicePosition];
        }
        // set results for a specific die element
        public void SetDiceResults(int iPosition)
        {
            iDiceResults[iPosition]++;
        }
        // get resulsts from a specific die element
        public int GetDiceResultsElement(int iPosition)
        {
            return iDiceResults[iPosition];
        }
        // get the iDiceResults array
        public int[] GetDiceResults()
        {
            return iDiceResults;
        }
        // reset iDiceResults's values to 0
        public void ResetDiceResults()
        {
            for (int i = 0; i < iDiceResults.Length; i++)
                iDiceResults[i] = 0;

        }
        
        //Set player's subscore
        public void SetSubScore(int iSetScore)
        {
            iSubScore = iSetScore;
        }
        //get player's subscore
        public int GetSubScore()
        {
            return iSubScore;
        }
        // reset player total score
        public void ReSetTotalScore()
        {
            iTotalScore = 0;
        }
        // set total score
        public void SetTotalScore()
        {
            iTotalScore += iSubScore;
        }
        // get total score
        public int GetTotalScore()
        {
            return iTotalScore;
        }
        //Set number of games player won
        public void SetGamesWon()
        {
            iGamesWon += 1;
        }
        // reset number of games player won to 0
        public void ResetGamesWon()
        {
            iGamesWon = 0;
        }
        //Get number of games player won
        public int GetGamesWon()
        {
            return iGamesWon;
        }
        
        // reset dice results, subscore and total score to 0
        public void ResetResults()
        {
            ResetDiceResults();
            SetSubScore(0);
            ReSetTotalScore();

        }
        // reset player, this will reset dice results, subscore and total score to 0
        // reset number of games by player to 0
        public void ResetPlayer()
        {
            ResetResults();
            ResetGamesWon();

        }
        #endregion
    }
}
