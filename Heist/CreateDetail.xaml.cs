using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
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
    public sealed partial class CreateDetail : Page
    {
        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        private PurchasedView rec;
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;
        string testlol;
        private List<ChapterView> list;
        public byte[] imgBuffer;
        public CreateDetail()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
                Frame.Navigate(typeof(CreateCollection));
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;

            try
            {
                rec = e.Parameter as PurchasedView;
                Cover.Source = rec.sel.Image;
                Title.Text = rec.sel.Title;
                Author.Text = rec.sel.Author;
                List<string> chaps = new List<string>();
                string[] lol = rec.purchases.Split(',');
                for (int i = 0; i < lol.Length; i++)
                {
                    string test3 = lol[i];
                    string[] test4 = test3.Split('.');
                    if (test4[0] == rec.sel.Id)
                    {
                        if (test4[1] != "full")
                            chaps.Add(test4[1]);
                        else
                        {
                            items = await Table.Where(Chapter
                => Chapter.bookid == rec.sel.Id).ToCollectionAsync();
                            foreach (Chapter lol2 in items)
                            {
                                chaps.Add(lol2.Id);

                            }
                        }
                    }
                }

                list = new List<ChapterView>();
                ChapterView temp;
                try
                {
                    items = await Table.Where(Chapter
                                => chaps.Contains(Chapter.Id)).ToCollectionAsync();


                    foreach (Chapter lol2 in items)
                    {
                        temp = new ChapterView();
                        temp.Id = lol2.Id;
                        temp.Title = lol2.Name;
                        temp.Price = "Price: " + lol2.price.ToString();
                        list.Add(temp);
                    }
                    LoadingBar.Visibility = Visibility.Collapsed;
                    StoreListView.ItemsSource = list;

                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    await (new MessageDialog("Can't get data now please try again later")).ShowAsync();
                }
            }

            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't get data now please try again later")).ShowAsync();
            }
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            MeriCollection ob = new MeriCollection();
            LoadingBar.Visibility = Visibility.Visible;
         
            LoadingBar.IsIndeterminate = true;
            try
            {
                items2 = await Table2.Where(Book
                                => Book.Id == rec.sel.Id).ToCollectionAsync();
                ob.BookId = rec.sel.Id;
                ob.UserName = testlol;
                foreach (Book lol in items2)
                {
                    ob.BookName = lol.Title;
                }
            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Something bad happened :(:(")).ShowAsync();
                return;
            }
            try
            {
                var test = sender as Button;
                var test2 = test.Parent as Grid;
                var test3 = test2.Children[2] as TextBlock;
                var test4 = test2.Children[0] as TextBlock;
                string nam = test4.Text;
                ob.ChapterNo = nam;
                ob.ChapterId = test3.Text;
                App.mc.Add(ob);
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Added Successful")).ShowAsync();
            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't Add now please try after sometime")).ShowAsync();
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

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private async void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("You are successfully loged out :):)")).ShowAsync();
            Frame.Navigate(typeof(Login));
        }
        private void MenuButton7_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyCollection));
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
            Frame.Navigate(typeof(CreateCollection));
        }
    }
}
