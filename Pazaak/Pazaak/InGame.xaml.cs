using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public InGame()
        {
            this.InitializeComponent();
            Card[] p = new Card[15];
            for (Int16 l = 0; l < 15; l++)
            {
                Card c = new Card(l);
                p[l] = c;

            }
            pl = new User(p, "Oliver");
            usrBlk.Text = pl.Name;
            popHndCrds();
            Card crd = new Card();
            pl.GotDk = false;
            pl.IsTrn = false;
            pl.turn(false, crd, usrScr, this);
        }
        public void popHndCrds()
        {
            usrHnd1.Content = pl.Hand[0].Val;
            usrHnd2.Content = pl.Hand[1].Val;
            usrHnd3.Content = pl.Hand[2].Val;
            usrHnd4.Content = pl.Hand[3].Val;
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Card c = new Card();
            pl.GotDk = false;
            pl.IsTrn = false;
            pl.turn(false, c, usrScr, this);
        }

        private void stnd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Card c = new Card();
            pl.turn(true, c, usrScr, this);
        }
        
        public void crdSwtch(Int16 val)
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

        private void crd1_tap(object sender, TappedRoutedEventArgs e)
        {
            pl.IsTrn = true;
            pl.GotDk = true;
            Button b = usrHnd1;
            pl.useCrd(b, this, usrScr);
        }
        private void crd2_tap(object sender, TappedRoutedEventArgs e)
        {
            pl.IsTrn = true;
            pl.GotDk = true;
            Button b = usrHnd2;
            pl.useCrd(b, this, usrScr);
        }
        private void crd3_tap(object sender, TappedRoutedEventArgs e)
        {
            pl.IsTrn = true;
            pl.GotDk = true;
            Button b = usrHnd3;
            pl.useCrd(b, this, usrScr);
        }
        private void crd4_tap(object sender, TappedRoutedEventArgs e)
        {
            pl.IsTrn = true;
            pl.GotDk = true;
            Button b = usrHnd4; 
            pl.useCrd(b, this, usrScr);
        }
        
    }
}
