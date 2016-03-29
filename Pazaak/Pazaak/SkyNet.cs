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
                    ScrTypePair fustMv = decideMove(this.CurrScr);
                    await handDeal(currMv.CrdType - 1, iG);
                    ScrTypePair futMv = decideMove(this.CurrScr);
                    //enemy will stand if its score is winning, and is as close to 20 as the difficulty allows
                    //will adjust and play more aggressively if for example the user is on 19, and the enemy is on 18
                    //enemy will play to win and try its hardest to get a higher score than the user
                    if (futMv.Score < App.gmDifficulty&&futMv.CrdType==0&&u.CurrScr<=this.CurrScr)
                    {
                        //stand
                        this.Stndng = true;
                    }
                }
                if(this.CurrScr>20)
                {
                    this.IsBust = true;
                }

                else if((currMv.Score < App.gmDifficulty || (this.CurrScr > u.CurrScr&&u.Stndng&&u.IsBust==false)) || this.CurrScr == 20)
                {
                    this.Stndng = true;
                }
            }
            enScr.Text = this.CurrScr.ToString();
            u.IsTrn = true;
        }
        private async Task handDeal(int hndNum, InGame iG)
        {
            if (this.Hand[hndNum].IsUsed == false)
            {
                prcesCrd(this.Hand[hndNum]);
                await iG.srchGrid(this);
                setHnd(hndNum); 
                
                iG.printHandCard(this.TrnCnt, Hand[hndNum].Val, this);
            }
        }

        private void setHnd(int hndNum)
        {
            Card c = Hand[hndNum];
            c.IsUsed = true;
            //Hand[hndNum].IsUsed = true;
        }

        private async Task deckDeal(InGame iG, Queue<Card> deck)
        {
            Card c = deck.Dequeue();
            prcesCrd(c);
            await(iG.srchGrid(this));
            iG.txtCardVal(c.Val, this);
        }

        private void prcesCrd(Card c)
        {   
            addCrd(c);
            TrnCnt++;
            CurrScr += c.Val;
        }

        public ScrTypePair decideMove(int currScr)
        {
            int[] crdVals = new int[this.Hand.Length+1];
            int currHiScr = 0;
            if(currScr>10)
            {
                crdVals[0] = 20- currScr;
            }
            else
            {
                crdVals[0] = 10;
            }
            //score the hand values
            for(int possLoop = 0; possLoop<this.Hand.Length; possLoop++)
            {
                int i = possLoop + 1;
                int score = this.Hand[possLoop].Val;
                if ((score < 0 && currScr > 20) || (score > 0 && currScr < 20))
                {
                    crdVals[i] = scrCrd(score, currScr);
                    if (crdVals[i] > crdVals[currHiScr])
                    {
                        currHiScr = i;
                    }
                }
                else
                {
                    crdVals[i] = -100;
                }
            }
            return new ScrTypePair(crdVals[currHiScr], currHiScr);
        }
        public int scrCrd(int crdVal, int currScr)
        {
            int s;
            if (crdVal > 0)
            {
                s = 20 - crdVal;
                s = s - currScr;
            }
            else
            {
                s = crdVal+currScr;
                s = 20 - s;
            }
            
            if (s < 0)
            {
                //dont use this card
                return -100;
            }
            else
            {
                if (s == 0)
                {
                    //use this card
                    return 100;
                }
                else
                {
                    //return crdVal
                    return 10 - s;
                }
            }
        }
    }
}
