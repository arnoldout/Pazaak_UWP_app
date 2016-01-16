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
        public InGame()
        {
            this.InitializeComponent();
            Card[] p = new Card[15];
            for (Int16 l = 0; l < 15; l++)
            {
                Card c = new Card(l);
                p[l] = c;
                
            }
            Player pl = new Player(p, "Oliver");
            usrBlk.Text = pl.Name;
        }
    }
}
