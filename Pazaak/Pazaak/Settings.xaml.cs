using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            //display the current user name in the textbox
            nmEnter.Text = App.usrName;
        }

        /*

            The difficulty values change how close to 20 the AI must be at before it will stick
            I.e on easy mode, the AI will stand more frequently but will have less of a chance to win, as it's 
            score will be lower

        */
        //hard difficulty
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 2;
            try
            {
                //try to set the user name from local settings
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["difficulty"] = App.gmDifficulty;
            }
            catch (Exception diffNotFound)
            {
                //user name defaults to "User Name"
                string errMsg = diffNotFound.Message;
            }

        }

        //medium difficulty
        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 3;
            try
            {
                //try to set the user name from local settings
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["difficulty"] = App.gmDifficulty;
            }
            catch (Exception diffNotFound)
            {
                //user name defaults to "User Name"
                string errMsg = diffNotFound.Message;
            }
        }
        //easy difficulty
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 4;
            try
            {
                //try to set the user name from local settings
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["difficulty"] = App.gmDifficulty;
            }
            catch (Exception diffNotFound)
            {
                //user name defaults to "User Name"
                string errMsg = diffNotFound.Message;
            }
        }
       
        //back button sends user back to the main page
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        //on text changed event, the value of the textbox will become the 
        //new user name that is displayed in game
        private void nmEnter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            App.usrName = tb.Text;
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["usrName"] = App.gmDifficulty;
            }
            catch (Exception usrNameNotFound)
            {
                //user name defaults to "User Name"
                string errMsg = usrNameNotFound.Message;
            }
        }
    }
}
