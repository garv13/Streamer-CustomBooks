using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices;

namespace Heist
{
    public sealed partial class MainPage : Page
    {
        private IMobileServiceTable<User> Table2 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items2;
        string testlol;
        public MainPage()
        {
            this.InitializeComponent();
            // getdata();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsActive = true;
            try {
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await folder.GetFileAsync("sample.txt");

                testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                items2 = await Table2.Where(User
                               => User.username == testlol).ToCollectionAsync();
                balance.Text = items2[0].wallet.ToString();
                LoadingBar.Visibility = Visibility.Collapsed;
            }
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Can't update data");
                await msgbox.ShowAsync();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Downloads));
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Purchased));
        }

        private void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Store));
        }

        private void MenuButton7_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyCollection));
        }
        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsActive = true;
            try
            {
                items2 = await Table2.Where(User
                          => User.username == testlol).ToCollectionAsync();
                User a = items2[0];
                if (!(Funds.Text.All(char.IsDigit)))
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Enter correct amount");
                    await msgbox.ShowAsync();
                    return;
                }

                a.wallet += int.Parse(Funds.Text);
                await Table2.UpdateAsync(a);
                MessageDialog msgbox1 = new MessageDialog("Money Added!!");
                LoadingBar.Visibility = Visibility.Collapsed;
                await msgbox1.ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Can't add money");
                await msgbox.ShowAsync();
            }
           
        }

        private async void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("You are successfully loged out :):)")).ShowAsync();
            Frame.Navigate(typeof(Login));
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Funds.Visibility = Visibility.Visible;
            MoneyAdd.Visibility = Visibility.Visible;
        }
    }
}
 