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
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }
        //hard difficulty
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 2;
        }

        //medium difficulty
        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 3;
        }
        //easy difficulty
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            App.gmDifficulty = 4;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
