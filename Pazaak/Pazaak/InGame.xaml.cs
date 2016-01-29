using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
        public InGame()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            usrImgs = new Image[9] { usrCrdImg1, usrCrdImg2, usrCrdImg3, usrCrdImg4, usrCrdImg5, usrCrdImg6, usrCrdImg7, usrCrdImg8, usrCrdImg9 };
            enImgs = new Image[9] { enCrdImg1, enCrdImg2, enCrdImg3, enCrdImg4, enCrdImg5, enCrdImg6, enCrdImg7, enCrdImg8, enCrdImg9 };
            Player[] arr = e.Parameter as Player[];
            if (arr != null)
            {
                en = (SkyNet)arr[0];
                pl = (User)arr[1];
                usrBtnsRset();
                usrScrSwch();
                enScrSwch();
                enBlk.Text = en.Name;
                usrBlk.Text = pl.Name;
                //make hands for the user and en
                popHndCrds();
                Card crd = new Card();
                mkDeck();
                //initially gives user a card
                int i = ((21 % 10) * 10);
                pl.deckCall(false, usrScr, this, deck);
                srchGrid(pl);
                //Storyboard.SetTargetName(mvCard, "image");
                //mvCard.Begin();
            }
        }
        public Image srchImg(String str, Image[] crds)
        {
            for(int i = 0; i<crds.Length; i++)
            {
                if(crds[i].Name==str)
                {
                    return crds[i];
                }
            }
            return null;
        }
        public async void srchGrid(Player p)
        {
            /*
                11 12 13
                21 22 23
                31 32 33
            */
            int currPos = 0;
            if (p.GetType() == typeof(User))
            {
                currPos = 33;
            }
            else if (p.GetType() == typeof(SkyNet))
            {
                currPos = 31;
            }
            int finalPos = getCurrGridSq(p.TrnCnt, p);

            if (finalPos != 0)
            {
                while (currPos != finalPos)
                {
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
                        //mvLeft
                        if (p.GetType() == typeof(User))
                        {
                            currPos--;
                        }
                        else if (p.GetType() == typeof(SkyNet))
                        {
                            currPos++;
                        }
                    }
                    if ((int)currPos / 10 > (int)finalPos / 10)
                    {
                        currPos -= 10;
                    }
                }
                Image i;
                if (p.GetType() == typeof(User))
                {
                    i = srchImg("usrCrdImg" + getImgSq(finalPos), getImgArr(p));
                }
                else
                {
                    i = srchImg("enCrdImg" + getImgSq(finalPos), getImgArr(p));
                }
                
                if (i != null)
                {
                    i.Opacity = 2;
                }
            }
        }
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
        private async Task animateCard(String imageNm, Player p)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimation da = new DoubleAnimation();
            Storyboard.SetTargetProperty(da, "Opacity");
            Storyboard.SetTarget(da, srchImg(imageNm, getImgArr(p)));
            da.From = 0;
            da.To = 2;
            da.AutoReverse = true;
            da.EnableDependentAnimation = true;
            da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75));
            sb.Children.Add(da);
            sb.Begin();
            await Task.Delay((int)da.Duration.TimeSpan.TotalMilliseconds / 2);
            Task t = new Task(async () =>
            {
                await Task.Delay((int)da.Duration.TimeSpan.TotalMilliseconds/2);
                sb.Stop();
            });
            
            sb.Stop();
        }
        private async void stopAnim(Storyboard sb)
        {
            await Task.Delay(1000);
            sb.Stop();
        }

        private void mkDeck()
        {
            int[] deckVals = new int[40] {1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10 };
            deckVals = Shuffle(deckVals);
            deck = new Queue<Card>();
            for (Int16 card = 1; card < 40; card++)
            {
                deck.Enqueue(new Card((Int16)deckVals[card]));
            }
        }
        
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
        public void showCard(Button b, Player p,  int placer)
        {
            b.Content = p.Hand[placer].Val;
        }
       
        public void popHndCrds()
        {
            for (int usrHndLoop = 0; usrHndLoop < pl.Hand.Length; usrHndLoop++)
            {
                if (usrHndLoop < en.Hand.Length)
                {
                    drawUsrHand(usrHndLoop);
                }
            }
            for (int enHndLoop = 0; enHndLoop<en.Hand.Length; enHndLoop++)
            {
                if(enHndLoop<en.Hand.Length)
                {
                    drawEnHand(enHndLoop);
                }
            }
            
        }
        public void drawUsrHand(int i)
        {
            Button hand;
            switch (i)
            {
                case 0:
                    hand = usrHnd1;
                    break;
                case 1:
                    hand = usrHnd2;
                    break;
                case 2:
                    hand = usrHnd3;
                    break;
                case 3:
                    hand = usrHnd4;
                    break;
                default:
                    hand = usrHnd1;
                    break;
            }
            showCard(hand, pl, i);
        }
        public void drawEnHand(int i)
        {
            Button hand;
            switch (i)
            {
                case 0:
                    hand = enHnd1;
                    break;
                case 1:
                    hand = enHnd2;
                    break;
                case 2:
                    hand = enHnd3;
                    break;
                case 3:
                    hand = enHnd4;
                    break;
                default:
                    hand = enHnd1;
                    break;
            }
            showCard(hand, en, i);
        }
        private async void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            popHndCrds();
            await endTurn();
            if(chkScrs())
            {
                newFrme();
            }
            usrBtnsRset();
            enStatus();
            pl.deckCall(false,  usrScr, this, deck);
            if (chkScrs())
            {
                newFrme();
            }
            else
            {
                srchGrid(pl);
            }
        }

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
            clrUsrBtns();
            if (chkScrs())
            {
                newFrme();
            }
            pl.GotDk = false;
            pl.IsTrn = false;
            //delaying the main process here gives the game a better flow
            //it slows down the deal
            //using process pausing also ensures that the random object dosnt keep generating the same cards
            await Task.Delay(100);
            en.mkMove(pl, enScr, this, deck);
            //delaying the main process gives the game the illusion that the AI is thinking
            await Task.Delay(1000);
        }

        public void enStatus()
        {
            if (en.CurrScr == 20)
            {
                enStsMsg.Text = en.Name+": Huzaa!";
            }
            else if(en.IsBust)
            {
                enStsMsg.Text = en.Name + ": Bust";
            }
            else if(en.Stndng)
            {
                enStsMsg.Text = en.Name + ": I Stand";
            }
            
        }
        private void stnd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            pl.GotDk = false;
            pl.IsTrn = false;
            pl.Stndng = true;
            stand();
        }

        public async void stand()
        {
            clrUsrBtns();
            Boolean over = false;
            while (en.Stndng == false && en.IsBust == false&&over==false)
            {

                await Task.Delay(1000);
                en.mkMove(pl, enScr, this, deck);
                enStatus();
                over = chkScrs();
            }
            newFrme();
        }

        public Boolean chkScrs()
        {
            if(en.TrnCnt>9)
            {
                en.rndWn(en);
                usrScrSwch();
                return true;
            }
            else if(pl.TrnCnt>9)
            {
                pl.rndWn(en);
                usrScrSwch();
                return true;
            }
            else if (en.IsBust)
            {
                //user wins
                pl.rndWn(en);
                usrScrSwch();
                return true;
            }
            else if (pl.IsBust)
            {
                //enemy wins
                en.rndWn(en);
                enScrSwch();
                return true;
            }
            else if (en.Stndng && pl.Stndng)
            {
                if (en.CurrScr > pl.CurrScr)
                {
                    //enemy Wins
                    en.rndWn(en);
                    enScrSwch();
                    return true;
                }
                else if (pl.CurrScr > en.CurrScr)
                {
                    //user wins
                    pl.rndWn(en);
                    usrScrSwch();
                    return true;
                }
                else
                {
                    //draw
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void usrScrSwch()
        {
            SolidColorBrush scb = new SolidColorBrush(Windows.UI.Colors.Red);
            //using code numbers to identify who's score has incremented was the easiest way i found to update the round ellipses
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
            SolidColorBrush scb = new SolidColorBrush(Windows.UI.Colors.Red);
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

        //clear the button's content
        public void clrUsrBtns()
        {
            endBtn.Content = "";
            stndBtn.Content = "";
        }

        public void clrEnHndCrd(int i)
        {
            switch(i)
            {
                case 0:
                    enHnd1.Content = "";
                    break;
                case 1:
                    enHnd2.Content = "";
                    break;
                case 2:
                    enHnd3.Content = "";
                    break;
                case 3:
                    enHnd4.Content = "";
                    break; 
                default:
                    break;
            }
        }
        
        //reset the Button's content
        public void usrBtnsRset()
        {
            endBtn.Content = "End Turn";
            stndBtn.Content = "Stand";
        }

        public void plCrdSwtch(Int16 val)
        {
            if (!pl.Stndng)
            {
                switch (pl.TrnCnt)
                {
                    case 0:
                        usrCrd1.Text = val.ToString();
                        break;
                    case 1:
                        usrCrd2.Text = val.ToString();
                        break;
                    case 2:
                        usrCrd3.Text = val.ToString();
                        break;
                    case 3:
                        usrCrd4.Text = val.ToString();
                        break;
                    case 4:
                        usrCrd5.Text = val.ToString();
                        break;
                    case 5:
                        usrCrd6.Text = val.ToString();
                        break;
                    case 6:
                        usrCrd7.Text = val.ToString();
                        break;
                    case 7:
                        usrCrd8.Text = val.ToString();
                        break;
                    case 8:
                        usrCrd9.Text = val.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        public void enCrdSwtch(Int16 val)
        {
            if (!en.Stndng)
            {
                switch (en.TrnCnt)
                {
                    case 0:
                        enCrd1.Text = val.ToString();
                        break;
                    case 1:
                        enCrd2.Text = val.ToString();
                        break;
                    case 2:
                        enCrd3.Text = val.ToString();
                        break;
                    case 3:
                        enCrd4.Text = val.ToString();
                        break;
                    case 4:
                        enCrd5.Text = val.ToString();
                        break;
                    case 5:
                        enCrd6.Text = val.ToString();
                        break;
                    case 6:
                        enCrd7.Text = val.ToString();
                        break;
                    case 7:
                        enCrd8.Text = val.ToString();
                        break;
                    case 8:
                        enCrd9.Text = val.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        private void crd1_tap(object sender, TappedRoutedEventArgs e)
        {
            Button b = usrHnd1;
            pl.handCall(b, this, usrScr);
            usrHnd1.Background = new SolidColorBrush(Windows.UI.Colors.White);
            usrHnd1.BorderThickness = new Thickness(0);
        }
        private void crd2_tap(object sender, TappedRoutedEventArgs e)
        {
            Button b = usrHnd2;
            pl.handCall(b, this, usrScr);
            usrHnd1.Background = new SolidColorBrush(Windows.UI.Colors.White);
            usrHnd1.BorderThickness = new Thickness(0);
        }
        private void crd3_tap(object sender, TappedRoutedEventArgs e)
        {
            Button b = usrHnd3;
            pl.handCall(b, this, usrScr);
            usrHnd1.Background = new SolidColorBrush(Windows.UI.Colors.White);
            usrHnd1.BorderThickness = new Thickness(0);
        }
        private void crd4_tap(object sender, TappedRoutedEventArgs e)
        {
            Button b = usrHnd4;
            pl.handCall(b, this, usrScr);
            usrHnd1.Background = new SolidColorBrush(Windows.UI.Colors.White);
            usrHnd1.BorderThickness = new Thickness(0);
        }

    }
}
