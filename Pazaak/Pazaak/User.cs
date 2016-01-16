using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Pazaak
{
    class User : Player
    {
        public User(Card[] crdPool, String name) : base(crdPool, name)
        {

        }
        public void tkTurn(Card c, Boolean stnd)
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
                
            }
        }
        public void turn(Boolean b, Card c, TextBlock usrScr, InGame iG)
        {
            if (this.CurrScr > 20)
            {
                b = true;
            }
            iG.crdSwtch(c.Val);
            tkTurn(c, b);
            usrScr.Text = this.CurrScr.ToString();
        }
        public void useCrd(Button b, InGame iG, TextBlock usrScr)
        {
            if (this.Stndng == false&&this.GotDk==true&&this.IsTrn&&!b.Content.Equals(""))
            {
                b.Background = new SolidColorBrush(Windows.UI.Colors.White);
                int val = (Int16)b.Content;
                Card c = new Card((Int16)val);
                turn(false, c, usrScr, iG);
                b.Content = "";
            }
        }

    }
}
