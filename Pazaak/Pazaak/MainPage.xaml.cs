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
            //populate the deck
            Card[] p = new Card[15];
            for (Int16 l = 0; l < 15; l++)
            {
                Int16 i = (Int16) (-1 * l);
                Card c = new Card(l);
                p[l] = c;

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
    }
}
