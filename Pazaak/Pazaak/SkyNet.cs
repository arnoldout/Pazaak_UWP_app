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

        public void mkMove(User u, TextBlock enScr, InGame iG, List<Card> deck)
        {
            
            //not the user's turn right now
            u.IsTrn = false;
            u.GotDk = false;
            if (TrnCnt < 9 && this.Stndng == false)
            {
                ScrTypePair currMv = decideMove(this.CurrScr);

                deckDeal(iG, deck);

                ScrTypePair futMv = decideMove(this.CurrScr);
                if (futMv.Score < 4)
                {
                    //stand
                    this.Stndng = true;
                }
                else if(currMv.CrdType!=0)
                {
                    //play hand card of value crdType-1
                    handDeal(currMv.CrdType-1, iG);
                }

                else if (this.CurrScr > 20)
                {
                    this.IsBust = true;
                }
            }
            enScr.Text = this.CurrScr.ToString();
            u.IsTrn = true;
        }
        private void handDeal(int hndNum, InGame iG)
        {
            prcesCrd(iG, this.Hand[hndNum]);
            this.Hand = this.Hand.Except(new Card[] { this.Hand[hndNum] }).ToArray();
        }

        private void deckDeal(InGame iG, List<Card> deck)
        {
            Card c = deck[0];
            deck.Remove(deck[0]);
            prcesCrd(iG, c);
        }

        private void prcesCrd(InGame iG, Card c)
        {   
            iG.enCrdSwtch(c.Val);
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
