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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pazaak
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SkyNet en;
        User pl;
        public MainPage()
        {
            this.InitializeComponent();
            App.posCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/CardPos.png"));
            App.negCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/CardNeg.png"));
            App.deckCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/DeckCard.png"));
            //populate the deck
            Card[] p = new Card[15];
            int cntTracker = 8;
            for (Int16 l = -8; l < 8; l++)
            {
                
                if (l != 0)
                {
                    Int16 i = (Int16)(-1 * l);
                    Card c = new Card(l);
                    p[l+cntTracker] = c;
                }
                else
                {
                    cntTracker--;
                }

            }
            //create new AI and user objects
            en = new SkyNet (p, "AI");
            pl = new User(p, "Oliver");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Player[] arr = new Player[2] {en, pl};
            this.Frame.Navigate(typeof(InGame), arr);
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
    }
}
