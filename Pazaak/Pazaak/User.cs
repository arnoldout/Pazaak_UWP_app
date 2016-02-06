using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Pazaak
{
    public class User : Player
    {
        public User(Card[] crdPool, String name) : base(crdPool, name)
        {

        }

        //called in the deckCall and the handCall methods
        //processes the card generated from deck or hand
        public void playCard(Card c, Boolean stnd, TextBlock tb)
        {
            if (stnd)
            {
                this.Stndng = true;
            }
            if (TrnCnt < 9 && this.Stndng == false&&IsTrn==true)
            {
                //IsTrn = false;
                addCrd(c);
                TrnCnt++;
                CurrScr += c.Val;
                tb.Text = this.CurrScr.ToString();
               /* if (CurrScr > 20)
                {
                    this.IsBust = true;
                }*/
            }
        }
        //user gets card from deck
        public async void deckCall(Boolean b, TextBlock usrScr, InGame iG, Queue<Card> deck)
        {
            /*if (this.CurrScr > 20)
            {
                b = true;
                this.IsBust = true;
                await iG.stand();
            }*/

            Card crd = deck.Dequeue();
            playCard(crd, b, usrScr);
            await iG.srchGrid(this);
            iG.txtCardVal(crd.Val, this);
            
        }
        // use a card from hand
        public async Task handCall(TextBlock tb, InGame iG, TextBlock usrScr)
        {
            //can only use a card if the card is still there
            if (this.Stndng == false && this.IsTrn && !tb.Text.Equals(""))
            {
                int val = Convert.ToInt16(tb.Text);
                Card crd = null;
                for (int i = 0; i < this.Hand.Length; i++)
                {
                    if (this.Hand[i].Val == (val))
                    {
                        //found card
                        crd = this.Hand[i];
                        this.Hand[i].IsUsed = true;
                        break;
                    }
                }
                playCard(crd, false, usrScr);
                iG.printHandCard(TrnCnt, crd.Val, this);
                //iG.txtCardVal(crd.Val, this);
                autoBust(iG);
                tb.Text = "";
                this.IsTrn = false;
            }
        }
        public Boolean autoBust(InGame iG)
        {
            if (this.CurrScr > 19)
            {
                if (this.CurrScr == 20)
                {
                    this.Stndng = true;
                    this.IsBust = false;
                }
                else
                {
                    this.IsBust = true;
                    this.Stndng = false;
                }
                return true;
            }
            return false;
        }

    }
}
