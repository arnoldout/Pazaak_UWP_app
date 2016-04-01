using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Pazaak
{
    public class SkyNet : Player
    {
        
        public SkyNet(Card[] crdPool, String name) : base(crdPool, name)
        {
          
        }

        public async Task mkMove(User u, TextBlock enScr, InGame iG, Queue<Card> deck)
        {

            //not the user's turn right now
            u.IsTrn = false;
            u.GotDk = false;
            
            if (TrnCnt < 9 && Stndng == false && IsBust==false)
            {
                await deckDeal(iG, deck);
                ScrTypePair currMv = decideMove(this.CurrScr);
                //refactor this, logic seems off
                if (currMv.CrdType != 0&&currMv.Score>App.gmDifficulty)
                {
                    //move is good, make move, then ponder future later
                    await handDeal(currMv.CrdType - 1, iG);
                    //simulate how the next turn could go
                    ScrTypePair futMv = decideMove(this.CurrScr);
                    //enemy will stand if its score is winning, and is as close to 20 as the difficulty allows
                    //will adjust and play more aggressively if for example the user is on 19, and the enemy is on 18
                    //enemy will play to win and try its hardest to get a higher score than the user
                    if (futMv.Score < App.gmDifficulty&&futMv.CrdType==0&&u.CurrScr<=this.CurrScr)
                    {
                        //if the future dosnt look good for the AI, it will stick
                        this.Stndng = true;
                    }
                }
                //if score is above 20, AI has bust
                if(this.CurrScr>20)
                {
                    this.IsBust = true;
                }
                //AI will stand if it evaluates to true
                else if((currMv.Score < App.gmDifficulty || (this.CurrScr > u.CurrScr&&u.Stndng&&u.IsBust==false)) || this.CurrScr == 20)
                {
                    this.Stndng = true;
                }
            }
            enScr.Text = this.CurrScr.ToString();
            u.IsTrn = true;
        }

        //process a card from the hand, make it dissapear from side, and appear on table
        private async Task handDeal(int hndNum, InGame iG)
        {
            if (this.Hand[hndNum].IsUsed == false)
            {
                prcesCrd(this.Hand[hndNum]);
                //animate hand card
                await iG.srchGrid(this);
                setHnd(hndNum); 
                
                iG.printHandCard(this.TrnCnt, Hand[hndNum].Val, this);
            }
        }

        //set hand card to isUsed
        private void setHnd(int hndNum)
        {
            Card c = Hand[hndNum];
            c.IsUsed = true;
            //Hand[hndNum].IsUsed = true;
        }
        
        //take card from deck, animate to table
        private async Task deckDeal(InGame iG, Queue<Card> deck)
        {
            Card c = deck.Dequeue();
            prcesCrd(c);
            await(iG.srchGrid(this));
            iG.txtCardVal(c.Val, this);
        }

        //add card to table, increment the turn counter, and adjust the current score accordingly
        private void prcesCrd(Card c)
        {   
            addCrd(c);
            TrnCnt++;
            CurrScr += c.Val;
        }

        public ScrTypePair decideMove(int currScr)
        {
            //crdVals contains the 5 possible moves for the AI
            //i.e the 4 hand cards and the deck card
            int[] crdVals = new int[this.Hand.Length+1];
            int currHiScr = 0;
            //score deck card initially based on how risky a deck card would be

            //if score is above 10, there is a risk involved, that risk will be determined here
            if(currScr>10)
            {
                crdVals[0] = 20- currScr;
            }
            //if below 10 there is no risk in taking a deck card
            else
            {
                crdVals[0] = 10;
            }
            //score the hand values
            for(int possLoop = 0; possLoop<this.Hand.Length; possLoop++)
            {
                //i is used to track position in array as loop starts at 0, but first place in array is full already
                int i = possLoop + 1;
                //value of the current card
                int score = this.Hand[possLoop].Val;
                //if card is negative and score is above 20, or card is positive and score is below 20, the card could be useful 
                if ((score < 0 && currScr > 20) || (score > 0 && currScr < 20))
                {
                    //store how useful the card is in the array
                    crdVals[i] = scrCrd(score, currScr);
                    //if new score is best option, currHiScr is also set to use this option
                    if (crdVals[i] > crdVals[currHiScr])
                    {
                        currHiScr = i;
                    }
                }
                //if card is no good, set score to -100, not worth using this card
                else
                {
                    crdVals[i] = -100;
                }
            }
            return new ScrTypePair(crdVals[currHiScr], currHiScr);
        }

        //this algorithm simulates what would happen to the current score
        //were the crdVal were played
        public int scrCrd(int crdVal, int currScr)
        {
            int scr;
            //the card is positive
            if (crdVal > 0)
            {
                //determine how risky this positive card is
                scr = 20 - crdVal;
                //determine how close the score is to 0
                //as 0 would be a perfect match
                scr = scr - currScr;
            }
            //card is negative
            else
            {
                //determine how risky adding the negative card to the score would be
                scr = crdVal+currScr;
                //determine how close the score is to 0
                //as 0 would be a perfect match
                scr = 20 - scr;
            }
            
            if (scr < 0)
            {
                //dont use this card, it will result in a bust
                return -100;
            }
            else
            {
                if (scr == 0)
                {
                    //use this card, it will get AI to 20
                    return 100;
                }
                else
                {
                    //return crdVal
                    return 10 - scr;
                }
            }
        }
    }
}
