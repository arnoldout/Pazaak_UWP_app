using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

            ResourceCandidate rc = 
                ResourceManager.Current.MainResourceMap.GetValue("Resources/txtDiff/Text",
                ResourceContext.GetForCurrentView());
            string foreground = rc  .ValueAsString;
            try
            {
                //try to set the user name from local settings
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["usrName"] != null)
                {
                    App.usrName = (String)localSettings.Values["usrName"];
                }
                //if not saved, user name defaults to User Name
                else
                {
                    App.usrName = "User Name";
                    localSettings.Values["usrName"] = App.usrName;
                }
                //same as above, but with difficulty setting
                if (localSettings.Values["difficulty"] != null)
                {
                    App.gmDifficulty = (int)localSettings.Values["difficulty"];
                }
                //if not saved, user name defaults to User Name
                else
                {
                    App.gmDifficulty = 4;
                    localSettings.Values["difficulty"] = App.gmDifficulty;
                }
            }
            catch (Exception settingNotFound)
            {
                //user name defaults to "User Name"
                string errMsg = settingNotFound.Message;
                App.usrName = "User Name";
                App.gmDifficulty = 4;
            }
            //loading the sources of the card images into memory
            App.posCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/CardPos.png"));
            App.negCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/CardNeg.png"));
            App.deckCard = new BitmapImage(new Uri(base.BaseUri, "/Resources/DeckCard.png"));

            //populate the card pool
            Card[] pool = createCardPool();

            //create new AI and user objects
            en = new SkyNet(pool, "AI");
            pl = new User(pool, App.usrName);
        }

        private static Card[] createCardPool()
        {
            //an array of cards is created by the addition of i to the count tracker
            Card[] pool = new Card[15];
            int cntTracker = 8;
            for (Int16 i = -8; i < 8; i++)
            {
                //the loop iterates 16 times, it will hit this if statement on 15 occurences 
                if (i != 0)
                {
                    Card c = new Card(i);
                    pool[i + cntTracker] = c;
                }
                //when i enters into the positive numbers, count tracker is decremented
                //allows the algorithm the ability to reach the final boxes in the array
                else
                {
                    cntTracker--;
                }
            }
            //the card pool is returned
            return pool;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //on click event sends user to an instance of the inGame page
            //an array of players is passed as a parameter
            Player[] arr = new Player[2] {en, pl};
            this.Frame.Navigate(typeof(InGame), arr);
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            //on click of the settings button sends the user to an instance of the settings page
            this.Frame.Navigate(typeof(Settings));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //on click of the settings button sends the user to an instance of the settings page
            this.Frame.Navigate(typeof(Instructions));
        }
    }
}
