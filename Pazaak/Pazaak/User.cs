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
        public void playCard(Card c, Boolean stnd)
        {
            if (stnd)
            {
                this.Stndng = true;
            }
            if (TrnCnt < 9 && this.Stndng == false)
            {
                addCrd(c);
                TrnCnt++;
                CurrScr += c.Val;
                if (CurrScr > 20)
                {
                    this.IsBust = true;
                }
            }
        }
        //user gets card from deck
        public async void deckCall(Boolean b, TextBlock usrScr, InGame iG, Queue<Card> deck)
        {
            if (this.CurrScr > 20)
            {
                b = true;
                this.IsBust = true;
                await iG.stand();
            }

            Card crd = deck.Dequeue();
            playCard(crd, b);
            iG.txtCardVal(crd.Val, this);
            usrScr.Text = this.CurrScr.ToString();
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
                        break;
                    }
                }
                playCard(crd, false);
                iG.printHandCard(this.TrnCnt, crd.Val, this);
                iG.txtCardVal(crd.Val, this);

                tb.Text = "";
                this.IsTrn = false;

                this.Hand = this.Hand.Except(new Card[] { crd }).ToArray();
                if (this.CurrScr > 19)
                {
                    if (this.CurrScr < 20)
                    {
                        this.Stndng = true;
                    }
                    else
                    {
                        this.IsBust = true;
                    }
                    await iG.stand();
                }

            }
        }

    }
}
