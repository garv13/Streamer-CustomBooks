using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Heist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCollection : Page
    {
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        string test;
        string testlol;
        List<StoreListing> li;
        List<string> lis;

        private List<StoreListing> StoreList;

        public CreateCollection()
        {
            this.InitializeComponent();
            li = new List<StoreListing>();
            lis = new List<string>();
            Loaded += CreateCollection_Loaded;
        }

        private async void CreateCollection_Loaded(object sender, RoutedEventArgs e)
        {
            
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            try
            {


                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
                testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                items = await Table.Where(User
                              => User.username == testlol).ToCollectionAsync();
                test = items[0].purchases;
                string[] test2 = test.Split(',');
                if (test.Length == 0)
                {
                    noPurchase.Text = "You have not purchased anything";
                    noPurchase.Visibility = Visibility.Visible;
                    LoadingBar.Visibility = Visibility.Collapsed;
                    goto ex;
                }
                for (int i = 0; i < test2.Length; i++)
                {
                    string test3 = test2[i];
                    string[] test4 = test3.Split('.');
                    lis.Add(test4[0]);
                }
                items2 = await Table2.Where(Book
                              => lis.Contains(Book.Id)).ToCollectionAsync();
                StoreListing temp;
                StoreList = new List<StoreListing>();
                foreach (Book lol in items2)
                {
                    temp = new StoreListing();
                    temp.Title = lol.Title;
                    temp.Author = lol.Author;
                    temp.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(lol.ImageUri2));
                    temp.Id = lol.Id;
                    temp.Price = lol.Price.ToString();
                    StoreList.Add(temp);
                }
                LoadingBar.Visibility = Visibility.Collapsed;

                StoreListView.ItemsSource = StoreList;
                ex:;
            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't Update Now")).ShowAsync();
            }
        }

        private void StoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            StoreListing sent = e.ClickedItem as StoreListing;
            PurchasedView p = new PurchasedView();
            p.purchases = test;
            p.sel = sent;
            //sent.Price = 50.ToString();
            Frame.Navigate(typeof(CreateDetail), p);
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


        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void MenuButton7_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyCollection));
        }
        private async void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("You are successfully loged out :):)")).ShowAsync();
            Frame.Navigate(typeof(Login));
        }
        private async void NextBar_Click(object sender, RoutedEventArgs e)
        {
            if (App.mc.Count != 0)
            {
                await (new MessageDialog("Please review books once")).ShowAsync();
                Frame.Navigate(typeof(CollectionSort));
            }
            else
                await (new MessageDialog("Select atleast one chapter")).ShowAsync();
        }
        private void BackBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyCollection));
        }
    }
}
