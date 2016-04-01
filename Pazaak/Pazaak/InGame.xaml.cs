using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pazaak
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InGame : Page
    {
        private User pl;
        private SkyNet en;
        private Queue<Card> deck;
        Image[] usrImgs;
        Image[] enImgs;
        Image[] enHndImgs;
        TextBlock[] enTbHnds;
        TextBlock[] enTxtBlks;
        TextBlock[] usrTxtBlks;
        public String status = null;
        Boolean endTurnTapped;
        Boolean standTapped;
        Boolean usedCard;
        public InGame()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //arrays containing various controls created at design time
            //helps to greatly improve efficiency when searching for control values 
            usrImgs = new Image[9] { usrCrdImg1, usrCrdImg2, usrCrdImg3, usrCrdImg4, usrCrdImg5, usrCrdImg6, usrCrdImg7, usrCrdImg8, usrCrdImg9 };
            enImgs = new Image[9] { enCrdImg1, enCrdImg2, enCrdImg3, enCrdImg4, enCrdImg5, enCrdImg6, enCrdImg7, enCrdImg8, enCrdImg9 };
            enTxtBlks = new TextBlock[9] { enCrd1, enCrd2, enCrd3, enCrd4, enCrd5, enCrd6, enCrd7, enCrd8, enCrd9 };
            usrTxtBlks = new TextBlock[9] { usrCrd1, usrCrd2, usrCrd3, usrCrd4, usrCrd5, usrCrd6, usrCrd7, usrCrd8, usrCrd9 };
            enHndImgs = new Image[4] { enHnd1, enHnd2, enHnd3, enHnd4 };
            enTbHnds = new TextBlock[4] { tbEnHnd1, tbEnHnd2, tbEnHnd3, tbEnHnd4 };

            //boolean values control access to buttons in game
            endTurnTapped = false;
            standTapped = false;
            usedCard = false;
            //take in a parameter of the two player objects created in the previous page.
            //I chose to take parameters to display my knowledge of multiple ways to pass data between pages
            Player[] arr = e.Parameter as Player[];
            if (arr != null)
            {
                //setting the players 
                en = (SkyNet)arr[0];
                pl = (User)arr[1];

                usrScrSwch();
                enScrSwch();
                //display name of object on screen
                enBlk.Text = en.Name;
                usrBlk.Text = pl.Name;
                //make hands for the user and en
                popHndCrds();
                //initialize the hand images
                initHands();
                //create an actual deck of cards
                mkDeck();
                //initially gives user a card
                pl.deckCall(false, usrScr, this, deck);
                //animates the user's deck card on screen
                await srchGrid(pl);
            }
        }
        public void initHands()
        {
            //display hand cards at correct positions 
            //with correct image source 
            setCardCol(tbHnd1.Text, usrHnd1);
            setCardCol(tbHnd2.Text, usrHnd2);
            setCardCol(tbHnd3.Text, usrHnd3);
            setCardCol(tbHnd4.Text, usrHnd4);
            setCardCol(tbEnHnd1.Text, enHnd1);
            setCardCol(tbEnHnd2.Text, enHnd2);
            setCardCol(tbEnHnd3.Text, enHnd3);
            setCardCol(tbEnHnd4.Text, enHnd4);
        }
        public void setCardCol(String val, Image i)
        {   
            //sets card source dependent on card value
            if (validateStrNum(val))
            {
                int digit = Convert.ToInt16(val);
                if (digit > 0)
                {
                    i.Source = App.posCard;
                }
                else
                {
                    i.Source = App.negCard;
                }
            }
        }
        public Boolean validateStrNum(String s)
        {
            //discovers if value is able to be converted to an integer 
            try
            {
                Convert.ToInt16(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //search Images to find str value as a name
        public Image srchImg(String str, Image[] crds)
        {
            for (int i = 0; i < crds.Length; i++)
            {
                if (crds[i].Name == str)
                {
                    return crds[i];
                }
            }
            return null;
        }
        public async Task srchGrid(Player p)
        {
            /*
                11 12 13
                21 22 23
                31 32 33

                Basis of the card animation, this method tracks the position of the card as it moves through the grid
                it moves the card horizontally and vertically through the grid, animating the card after every grid shift

                i.e: if the card is moving left, starting at 33 and wants to get to position 11 as shown above, 
                the card will animate at 33, then shift to 22, where it will animate again, and finally move to 11, 
                where it will be given an image source and become a static image
            */
            int currPos = 0;

            //the user card moves left from 33
            if (p.GetType() == typeof(User))
            {
                currPos = 33;
            }
            //the AI moves right from 31
            else if (p.GetType() == typeof(SkyNet))
            {
                currPos = 31;
            }
            
            //the final position the image needs to move toward
            int finalPos = getCurrGridSq(p.TrnCnt, p);

            if (finalPos != 0)
            {
                while (currPos != finalPos)
                {
                    //animate card to current position
                    if (p.GetType() == typeof(User))
                    {
                        
                        await animateCard("usrCrdImg" + getImgSq(currPos), p);
                    }
                    else if (p.GetType() == typeof(SkyNet))
                    {
                        int y = getImgSq(currPos);
                        await animateCard("enCrdImg" + getImgSq(currPos), p);
                    }

                    if (currPos % 10 != finalPos % 10)
                    {
                        //user needs a left shift
                        if (p.GetType() == typeof(User))
                        {
                            currPos--;
                        }
                        //AI needs a right shift
                        else if (p.GetType() == typeof(SkyNet))
                        {
                            currPos++;
                        }
                    }
                    //both card types need to move up
                    if ((int)currPos / 10 > (int)finalPos / 10)
                    {
                        currPos -= 10;
                    }
                }
                //grab image source
                Image img;
                if (p.GetType() == typeof(User))
                {
                    img = srchImg("usrCrdImg" + getImgSq(finalPos), getImgArr(p));
                }
                else
                {
                    img = srchImg("enCrdImg" + getImgSq(finalPos), getImgArr(p));
                }

                if (img != null)
                {
                    //display card image
                    img.Opacity = 2;
                    //img.Source = App.deckCard;
                }
            }
        }
        //gets the appropraite array based on the Player type
        public Image[] getImgArr(Player p)
        {
            if (p.GetType() == typeof(User))
            {
                return usrImgs;
            }
            else
            {
                return enImgs;
            }
        }

        //converts the grid square into a table positioin
        public int getImgSq(int gridSq)
        {
            /*
                11 12 13
                21 22 23
                31 32 33
            */
            switch (gridSq)
            {
                case 11:
                    return 1;
                case 12:
                    return 2;
                case 13:
                    return 3;
                case 21:
                    return 4;
                case 22:
                    return 5;
                case 23:
                    return 6;
                case 31:
                    return 7;
                case 32:
                    return 8;
                case 33:
                    return 9;
                default:
                    return 0;
            }
        }
        //converts a table position into a grid square position
        public int getCurrGridSq(int trnCnt, Player p)
        {
            /*
                11 12 13
                21 22 23
                31 32 33
            */
            switch (trnCnt)
            {
                case 1:
                    return 11;
                case 2:
                    return 12;
                case 3:
                    return 13;
                case 4:
                    return 21;
                case 5:
                    return 22;
                case 6:
                    return 23;
                case 7:
                    return 31;
                case 8:
                    return 32;
                case 9:
                    return 33;
                default:
                    return 0;
            }
        }
        
        //a dynamic story board that uses tasks to auto complete themselves when the duration is complete
        private async Task animateCard(String imageNm, Player p)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();
            Storyboard.SetTargetProperty(da, "Opacity");
            Storyboard.SetTarget(da, srchImg(imageNm, getImgArr(p)));
            srchImg(imageNm, getImgArr(p)).Source = new BitmapImage(new Uri(base.BaseUri, "/Resources/CardBack.png"));
            da.From = 0;
            da.To = 2;
            da.AutoReverse = true;
            da.EnableDependentAnimation = true;
            da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 50));
            sb.Children.Add(da);
            sb.Begin();
            await Task.Delay((int)da.Duration.TimeSpan.TotalMilliseconds);
            sb.Stop();
        }

        //create a deck of 40 cards that have randomized shuffled positions
        private void mkDeck()
        {
            int[] deckVals = new int[40] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            deckVals = Shuffle(deckVals);
            deck = new Queue<Card>();
            for (Int16 card = 1; card < 40; card++)
            {
                deck.Enqueue(new Card((Int16)deckVals[card]));
            }
        }

        //shuffle array
        public int[] Shuffle(int[] crdVals)
        {
            Random r = new Random();
            int counter = crdVals.Length;
            //looping through the list from bottom to top
            while (counter > 1)
            {
                counter--;
                //randomly select a number 
                int val1 = r.Next(counter + 1);
                //numer in list is saved in a temporary Card object
                int val2 = crdVals[val1];
                //curren
                crdVals[val1] = crdVals[counter];
                crdVals[counter] = val2;
            }
            return crdVals;
        }

        //display hand value on hand card
        public void showUsrCardValue(TextBlock tb, Player p, int placer)
        {
            tb.Text = p.Hand[placer].Val.ToString();
        }

        //draw all hand cards to screen
        public void popHndCrds()
        {
            for (int usrHndLoop = 0; usrHndLoop < pl.Hand.Length; usrHndLoop++)
            {
                drawUsrHand(usrHndLoop);
            }
            for (int enHndLoop = 0; enHndLoop < en.Hand.Length; enHndLoop++)
            {
                drawEnHand(enHndLoop);
            }
        }

        //draw card at corresponding position
        public void drawUsrHand(int i)
        {
            TextBlock tBlock;
            switch (i)
            {
                case 0:
                    tBlock = tbHnd1;
                    break;
                case 1:
                    tBlock = tbHnd2;
                    break;
                case 2:
                    tBlock = tbHnd3;
                    break;
                case 3:
                    tBlock = tbHnd4;
                    break;
                default:
                    tBlock = tbHnd1;
                    break;
            }

            if (pl.Hand[i].IsUsed == false)
            {
                showUsrCardValue(tBlock, pl, i);
            }
        }
        //draw enemy card to corresponding position
        public void drawEnHand(int i)
        {
            TextBlock hand;
            switch (i)
            {
                case 0:
                    hand = tbEnHnd1;
                    break;
                case 1:
                    hand = tbEnHnd2;
                    break;
                case 2:
                    hand = tbEnHnd3;
                    break;
                case 3:
                    hand = tbEnHnd4;
                    break;
                default:
                    hand = tbEnHnd1;
                    break;
            }
            if (en.Hand[i].IsUsed == false)
            {
                showUsrCardValue(hand, en, i);
            }
            
        }

        //tap event to end turn
        private async void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if its user's turn and end turn hasnt already been tapped this turn
            if (!endTurnTapped&&!(pl.Stndng))
            {
                //lock access to button
                endTurnTapped = true;
                //stop user's round if true
                if (pl.autoBust(this))
                {
                    //show end round message if both parties are done
                    if (chkScrs())
                    {
                        await showMsg();
                        newFrme();
                    }
                    //otherwise stand and wait for opponent to complete their move
                    else
                    {
                        await stand();
                    }
                }
                else
                {
                    //otherwise just end the turn
                    await endTurn();
                    //once AI is finished their turn, give user another card
                    pl.deckCall(false, usrScr, this, deck);
                }
                //unlock access to button
                endTurnTapped = false;
                usedCard = false;
            }
        }
        //this resets the players and starts a new round
        private async void newFrme()
        {
            await Task.Delay(500);
            pl.reset();
            en.reset();
            Player[] arr = new Player[2] { en, pl };
            Frame.Navigate(typeof(InGame), arr);
        }

        public async Task endTurn()
        {
            pl.GotDk = false;
            pl.IsTrn = false;
            //delaying the main process here gives the game a better flow
            //it slows down the deal
            //using process pausing also ensures that the random object dosnt keep generating the same cards
            await Task.Delay(100);
            await en.mkMove(pl, enScr, this, deck);
            if (!en.Stndng)
            {
                //delaying the main process gives the game the illusion that the AI is thinking
                await Task.Delay(1000);
            }

       }    
        public async Task showMsg()
        {
            /*
            * I got some of the following code about the content dialog boxes from here
            * http://www.reflectionit.nl/blog/2015/windows-10-xaml-tips-messagedialog-and-contentdialog
            */

            //using localisation to retrieve strings that will be displayed in the end round/game message

            ResourceCandidate rc;
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/ok",
                ResourceContext.GetForCurrentView());
            string ok = rc.ValueAsString;
            var message = new ContentDialog()
            {
                Title = status + "\n"+App.usrName+": " + pl.CurrScr + " AI: " + en.CurrScr,
                PrimaryButtonText = ok,
                
            };

            rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/newGm",
            ResourceContext.GetForCurrentView());
            string newGame = rc.ValueAsString;

            //if the message asks for a new game, the user is given an extra option 
            //they can select cancel, which will return them to the main menu
            if (status.Equals(newGame))
            {
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/cancel",
                ResourceContext.GetForCurrentView());
                string cancel = rc.ValueAsString;
                //secondary button added to message dialog
                message.SecondaryButtonText = cancel;
                //store which button was pressed
                var result = await message.ShowAsync();
                //if user wants another game, reset and start a new game
                if (result == ContentDialogResult.Primary)
                {
                    resetScrCircles();
                    en.hardReset();
                    pl.hardReset();
                    newFrme();
                }
                //otherwise return user to main page
                else if(result == ContentDialogResult.Secondary)
                {
                    //navigate to home page
                    Frame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                //show message normally if not end of game
                //await feedback and start a new round
                await message.ShowAsync();
                newFrme();
            }
        }
        //tap event for stand button
        private async void stnd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //standTapped controls access to button after its been tapped
            if (pl.IsTrn&&!standTapped)
            {
                standTapped = true;
                //user is bust if score is over 20
                if(pl.CurrScr>20)
                {
                    pl.IsBust = true;
                }
                pl.GotDk = false;
                pl.IsTrn = false;
                pl.Stndng = true;
                //await the stand method
                await stand();
                //unlock access to button
                standTapped = false;
            }
        }

        //stand method will simulate the AI's moves until it sticks or busts
        public async Task stand()
        {
            Boolean over = false;
            //loop until the round is over
            while (!over)
            {
                //await 1000 milliseconds to give illusion that AI is thinking
                await Task.Delay(1000);
                //AI makes move
                await en.mkMove(pl, enScr, this, deck);
                //check if round is over
                over = chkScrs();
            }
            //when loop exits, display the message
            await showMsg();
            
        }

        //check all the possible outcomes
        //return a boolean to indicate if round is over
        public Boolean chkScrs()
        {
            Boolean b = false;
            ResourceCandidate rc;

            if (!en.IsBust&&!pl.IsBust&&!en.Stndng&&!pl.Stndng)
            {
                //alter status string with a message to be displayed
                status = App.usrName +": " + pl.CurrScr + " AI: " + en.CurrScr;
                b = false;
            }
            //if enemy has filled up the table, it wins automatically if it has a valid score
            else if (en.TrnCnt > 8&&en.CurrScr<21)
            {
                
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/lost",
                ResourceContext.GetForCurrentView());
                string lost = rc.ValueAsString;
                status = lost;
                en.rndWn(en, this);
                usrScrSwch();
                
                b = true;
            }
            //if user has filled up the table, automatic win if it has a valid score
            else if (pl.TrnCnt > 8&&pl.CurrScr<21)
            {
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/win",
                ResourceContext.GetForCurrentView());
                string win = rc.ValueAsString;
                status = win;
                pl.rndWn(en, this);
                usrScrSwch();
               
                b = true;
            }
            //if enemy has gne over 20
            else if (en.IsBust)
            {
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/win",
                ResourceContext.GetForCurrentView());
                string win = rc.ValueAsString;
                status = win;
                //user wins
                pl.rndWn(en, this);
                usrScrSwch();
                
                b = true;
            }
            //if user has gone over 20
            else if (pl.IsBust)
            {
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/bust",
                ResourceContext.GetForCurrentView());
                string bust = rc.ValueAsString;
                status = bust;
                //enemy wins
                en.rndWn(en, this);
                enScrSwch();
                
                b = true;
            }
            //if both are standing
            else if (en.Stndng && pl.Stndng)
            {
                if (en.CurrScr > pl.CurrScr)
                {
                    //enemy Wins
                    rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/lost",
                    ResourceContext.GetForCurrentView());
                    string lose = rc.ValueAsString;
                    status = lose;
                    en.rndWn(en, this);
                    enScrSwch();
                    
                    b = true;
                }
                else if (pl.CurrScr > en.CurrScr)
                {
                    //user wins
                    rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/win",
                    ResourceContext.GetForCurrentView());
                    string win = rc.ValueAsString;
                    //user wins
                    status = win;
                    pl.rndWn(en, this);
                    usrScrSwch();
                    
                    b = true;
                }
                //tie game
                else
                {
                    rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/tie",
                    ResourceContext.GetForCurrentView());
                    string tie = rc.ValueAsString;

                    //draw
                    status = tie;
                    b = true;
                }
            }
            //tie game
            if(en.CurrScr==20&&pl.CurrScr==20)
            {
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/tie",
                    ResourceContext.GetForCurrentView());
                string tie = rc.ValueAsString;

                //draw
                status = tie;
                b = true;
            }
           
            return b;
        }

        private void usrScrSwch()
        {
            //colour rounds won circle
            SolidColorBrush scb = new SolidColorBrush(Windows.UI.Colors.DarkGreen);
            switch (pl.RndsWn)
            {
                //user score is 3
                case 3:
                    plCrcl3.Fill = scb;
                    goto case 2;
                //user score is 2
                case 2:
                    plCrcl2.Fill = scb;
                    goto case 1;
                //user score is 1
                case 1:
                    plCrcl1.Fill = scb;
                    break;
            }
        }

        private void enScrSwch()
        {
            //colour rounds won circle
            SolidColorBrush scb = new SolidColorBrush(Windows.UI.Colors.DarkGreen);
            //using code numbers to identify who's score has incremented was the easiest way i found to update the round ellipses
            switch (en.RndsWn)
            {
                //enemy score is 3
                case 3:
                    enCrcl3.Fill = scb;
                    goto case 2;
                //enemy score is 2
                case 2:
                    enCrcl2.Fill = scb;
                    goto case 1;
                //enemy score is 1
                case 1:
                    enCrcl1.Fill = scb;
                    break;
            }
        }
        public void resetScrCircles()
        {
            SolidColorBrush scb = new SolidColorBrush(Windows.UI.Colors.White);
            //using code numbers to identify who's score has incremented was the easiest way i found to update the round ellipses
            enCrcl3.Fill = scb;
            enCrcl2.Fill = scb;
            enCrcl1.Fill = scb;
            plCrcl3.Fill = scb;
            plCrcl2.Fill = scb;
            plCrcl1.Fill = scb;
        }
       
         public void printHandCard(int trnCnt, int crdVal, Player p)
         {
            //print hand cards to table
            BitmapSource bmS = chkCardSign(crdVal);
            Type t = chkImgTurn(p);
            if (t == typeof(User))
            {
                Image thisImg = srchImg("usrCrdImg" + trnCnt, usrImgs);
                srchTxtBlks("usrCrd" + (pl.TrnCnt), usrTxtBlks).Text = crdVal.ToString();
                setImg(thisImg, bmS);
            }
            else
            {
                Image thisImg = srchImg("enCrdImg" + trnCnt, enImgs);
                srchTxtBlks("enCrd" + (en.TrnCnt), enTxtBlks).Text = crdVal.ToString();
                setImg(thisImg, bmS);
                rmvEnHnd(crdVal.ToString());
            }
        }

        //remove hand from side
        public void rmvEnHnd(String crd)
        {
            for(int i = 0; i< enTbHnds.Length; i++)
            {
                if(enTbHnds[i].Text.Equals(crd))
                {
                    enHndImgs[i].Opacity = 0;
                    enTbHnds[i].Text = "";
                }
            }
        }
        //display an image 
        public void setImg(Image i, BitmapSource bms)
        {
            i.Source = bms;
            i.Opacity = 2;
        }


        public Type chkImgTurn(Player p)
        {
            if (p.GetType() == typeof(User))
            {
                return typeof(User);
            }
            else
            {
                return typeof(SkyNet);
            }
        }

        //check the sign of the selected card
        public BitmapSource chkCardSign(int crdVal)
        {
            if (crdVal > 0)
            {
                return App.posCard;
            }
            else
            {
                return App.negCard;
            }
        }
       
        //match the textblock to the name
        public TextBlock srchTxtBlks(String block, TextBlock[] blocks)
        {
            for(int counter = 0; counter<blocks.Length; counter++)
            {
                if(blocks[counter].Name.Equals(block))
                {
                    return blocks[counter];
                }
            }
            return null;
        }

        //during animation, the moving card image is set to an animation image
        //i.e the App.deckCard source
        public void txtCardVal(Int16 val, Player p)
        {
            if (!p.Stndng)
            {
                if (p.GetType() == typeof(SkyNet))
                {
                    srchTxtBlks("enCrd" + (en.TrnCnt), enTxtBlks).Text = val.ToString();
                    srchImg("enCrdImg" + (en.TrnCnt), enImgs).Source = App.deckCard;
                }
                else
                {
                    srchTxtBlks("usrCrd" + (pl.TrnCnt), usrTxtBlks).Text = val.ToString();
                    srchImg("usrCrdImg" + (pl.TrnCnt), usrImgs).Source = App.deckCard;
                }
            }
        }


        /*
            Events to move hand card onto table
            and also remove card from hand
        */
        private async void crd1_tap(object sender, TappedRoutedEventArgs e)
        {
            if (usedCard == false)
            {
                usedCard = true;
                TextBlock b = tbHnd1;
                await pl.handCall(b, this, usrScr);
                usrHnd1.Opacity = 0;
            }
        }
        private async void crd2_tap(object sender, TappedRoutedEventArgs e)
        {
            if (usedCard == false)
            {
                usedCard = true;
                TextBlock b = tbHnd2;
                await pl.handCall(b, this, usrScr);
                usrHnd2.Opacity = 0;
            }
        }
        private async void crd3_tap(object sender, TappedRoutedEventArgs e)
        {
            if (usedCard == false)
            {
                usedCard = true;
                TextBlock b = tbHnd3;
                await pl.handCall(b, this, usrScr);
                usrHnd3.Opacity = 0;
            }
        }
        private async void crd4_tap(object sender, TappedRoutedEventArgs e)
        {
            if (usedCard == false)
            {
                usedCard = true;
                TextBlock b = tbHnd4;
                await pl.handCall(b, this, usrScr);
                usrHnd4.Opacity = 0;
            }
        }
    }
}   
