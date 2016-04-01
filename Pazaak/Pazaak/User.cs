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
        //User is a Player
        //a user dosnt have its own variables, but it has lots of its own methods

        //base() creates a player object using the super constructor
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
                //when a card is played (can be deck or hand)
                //card is added to the table
                addCrd(c);
                //turn count is incremented
                TrnCnt++;
                //score is incremented by the value of the card
                CurrScr += c.Val;
                //write the new score to the screen
                tb.Text = this.CurrScr.ToString();
            }
        }
        //user gets card from deck
        public async void deckCall(Boolean b, TextBlock usrScr, InGame iG, Queue<Card> deck)
        {
            //the top card is taken from the deck
            Card crd = deck.Dequeue();
            //that card is played
            playCard(crd, b, usrScr);
            //card is then animated to the screen
            await iG.srchGrid(this);
            //value of card is displayed in the image of the card
            iG.txtCardVal(crd.Val, this);
            
        }
        // use a card from hand
        public async Task handCall(TextBlock tb, InGame iG, TextBlock usrScr)
        {
            //can only use a card if the card is still there
            if (this.Stndng == false && this.IsTrn && !tb.Text.Equals(""))
            {
                //grab the value of the card from the selected card's textBlock
                int val = Convert.ToInt16(tb.Text);
                Card crd = null;
                //searching hand array to find User's selected card, 
                //place that card on the table, mark as used
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
                //play the hand card
                playCard(crd, false, usrScr);
                iG.printHandCard(TrnCnt, crd.Val, this);
                //checking score, user will automatically stand or bust if score is 20 or more after a hand card
                autoBust(iG);
                //no need for that pesky card value to stay on screen anymore
                tb.Text = "";
            }
        }
        public Boolean autoBust(InGame iG)
        {
            if (this.CurrScr > 19)
            {
                //if the user has a score of 20, user's score is set 
                //game auto sets user to standing
                if (this.CurrScr == 20)
                {
                    this.Stndng = true;
                    this.IsBust = false;
                }
                //if score is above 20, then the user has bust
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
