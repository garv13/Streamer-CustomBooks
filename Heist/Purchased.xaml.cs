using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Heist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Purchased : Page
    {
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;

        private IMobileServiceTable<Collections> Table3 = App.MobileService.GetTable<Collections>();
        private MobileServiceCollection<Collections, Collections> items3;
        string test;
        string testlol;
        List<StoreListing> li;
        List<string> lis;

        private List<StoreListing> StoreList;
        

        public Purchased()
        {
            this.InitializeComponent();
            li = new List<StoreListing>();
            lis = new List<string>();
            Loaded += Purchased_Loaded;
        }

        private async void Purchased_Loaded(object sender, RoutedEventArgs e)
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
                if(test.Length == 0)
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

                test = items[0].collections;
                test2 = test.Split(',');
                if (test.Length == 0)
                {
                    noPurchase1.Text = "You have not purchased any collection";
                    noPurchase1.Visibility = Visibility.Visible;
                    LoadingBar1.Visibility = Visibility.Collapsed;
                    goto ex2;
                }
                for (int i = 0; i < test2.Length; i++)
                {
                         
                    lis.Add(test2[i]);
                }
                items3 = await Table3.Where(Collections
                              => lis.Contains(Collections.Id)).ToCollectionAsync();
                
                List<StoreListing> StoreList2 = new List<StoreListing>();
                foreach (Collections lol in items3)
                {
                    temp = new StoreListing();
                    temp.Title = lol.Name;
                    temp.Author = lol.CreatedBy;
                    temp.Image = new BitmapImage(new Uri(this.BaseUri, "Assets/BooksCollections.png"));
                    temp.Id = lol.Id;
                    temp.Price = "0";
                    StoreList2.Add(temp);
                }


                LoadingBar.Visibility = Visibility.Collapsed;

                StoreListView.ItemsSource = StoreList;
               
                LoadingBar1.Visibility = Visibility.Collapsed;

                StoreListView1.ItemsSource = StoreList2;
                ex:;
                ex2:;
            }
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't Update Now")).ShowAsync();
            }
        }


        //private async void load_Collectios()
        //{
        //    try
        //    {
        //        StorageFolder mainFol = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);
        //        if (mainFol != null)
        //        {
        //            StorageFile file = await mainFol.GetFileAsync("collJson.txt");
        //            string str = await FileIO.ReadTextAsync(file);
        //            CollJson ob1;
        //            ob1 = JsonConvert.DeserializeObject<CollJson>(str);
        //            ob1.first();

        //            items2 = await Table2.Where(Book
        //                    => lis.Contains(Book)).ToCollectionAsync();

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        await (new MessageDialog("No collections Purchased")).ShowAsync();
        //    }
        //}

        private void StoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            StoreListing sent = e.ClickedItem as StoreListing;
            PurchasedView p = new PurchasedView();
            p.purchases = test;
            p.sel = sent;
            //sent.Price = 50.ToString();
            Frame.Navigate(typeof(Purchaseddetail), p);
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
    }
}
