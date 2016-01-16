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
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            turn(false);
        }

        private void stnd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            turn(true);
        }
        public void turn(Boolean b)
        {
            Card c = new Card();
            pl.tkTurn(c, b);
            usrScr.Text = pl.CurrScr.ToString();
        }
    }
}
