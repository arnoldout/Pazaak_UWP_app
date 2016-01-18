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
                if(CurrScr>20)
                {
                    this.IsBust = true;
                }
            }
        }
        //user gets card from deck
        public void deckCall(Boolean b, Card c, TextBlock usrScr, InGame iG)
        {
            if (this.CurrScr > 20)
            {
                b = true;
                this.IsBust = true;
                iG.stand();
            }
            iG.plCrdSwtch(c.Val);
            playCard(c, b);
            usrScr.Text = this.CurrScr.ToString();
        }
        // use a card from hand
        public void handCall(Button b, InGame iG, TextBlock usrScr)
        {
            //can only use a card if the card is still there
            if (this.Stndng == false&&this.IsTrn&&!b.Content.Equals(""))
            {
                b.Background = new SolidColorBrush(Windows.UI.Colors.White);
                int val = (Int16)b.Content;
                Card c = new Card((Int16)val);
                deckCall(false, c, usrScr, iG);
                b.Content = "";
                this.IsTrn = false;
                if(this.CurrScr>20)
                {
                    this.IsBust = true;
                    iG.stand();
                }
            }
        }

    }
}
