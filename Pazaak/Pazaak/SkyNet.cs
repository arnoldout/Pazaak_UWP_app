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
            if (TrnCnt < 9 && this.Stndng == false)
            {
                ScrTypePair currMv = decideMove(this.CurrScr);

                await deckDeal(iG, deck);
                ScrTypePair futMv = decideMove(this.CurrScr);
                if (this.CurrScr > 20)
                {
                    this.IsBust = true;
                }
                else if (futMv.Score < 9)
                {
                    //stand
                    this.Stndng = true;
                }
                else if (currMv.CrdType != 0)
                {
                    //play hand card of value crdType-1
                    await handDeal(currMv.CrdType - 1, iG);
                }
            }
            enScr.Text = this.CurrScr.ToString();
            u.IsTrn = true;
        }
        private async Task handDeal(int hndNum, InGame iG)
        {
            prcesCrd(iG, this.Hand[hndNum]);
            await iG.srchGrid(this);
            iG.printHandCard(this.TrnCnt, Hand[hndNum].Val, this);
            this.Hand = this.Hand.Except(new Card[] { this.Hand[hndNum] }).ToArray();
        }

        private async Task deckDeal(InGame iG, Queue<Card> deck)
        {
            Card c = deck.Dequeue();
            prcesCrd(iG, c);
            await(iG.srchGrid(this));
            iG.txtCardVal(c.Val, this);
        }

        private void prcesCrd(InGame iG, Card c)
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
            currHiScr = 0;
            //score the hand values
            for(int possLoop = 0; possLoop<this.Hand.Length; possLoop++)
            {
                int i = possLoop + 1;
                int score = this.Hand[possLoop].Val;
                //crdVals[i] = scrCrd(score, currScr);
                crdVals[i] = scrCrd(score, currScr);
                if (crdVals[i]>crdVals[currHiScr])
                {
                    currHiScr = i;
                }
            }
            return new ScrTypePair(crdVals[currHiScr], currHiScr);
        }
        public int scrCrd(int crdVal, int currScr)
        {
            int s = 20 - crdVal;
            s = s - currScr;
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
