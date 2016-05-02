using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class Store : Page
    {
        private IMobileServiceTable<Book> Table = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items;

        private IMobileServiceTable<Collections> Table2 = App.MobileService.GetTable<Collections>();
        private MobileServiceCollection<Collections, Collections> items2;


        private List<string> BookNames;
        private List<string> CollNames;

        private List<StoreListing> StoreList;
        public Store()
        {
            this.InitializeComponent();
            Loaded += Store_Loaded;
        }

        private async void Store_Loaded(object sender, RoutedEventArgs e)
        {
            Box.Visibility = Visibility.Collapsed;
            SearchButton.Visibility = Visibility.Collapsed;
            StoreListView.Visibility = Visibility.Collapsed;

            Box2.Visibility = Visibility.Collapsed;
            SearchButton2.Visibility = Visibility.Collapsed;
            StoreListView2.Visibility = Visibility.Collapsed;


            BookNames = new List<string>();
            CollNames = new List<string>();

            StoreList = new List<StoreListing>();


            LoadingBar.IsIndeterminate = true;
            StoreListing temp;
            try
            {
                items = await Table.Where(Book
                        => Book.IsReady == true).ToCollectionAsync();
                foreach (Book lol in items)
                {
                    temp = new StoreListing();
                    BookNames.Add(lol.Title);
                    temp.Title = lol.Title;
                    temp.Author = lol.Author;
                    temp.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(lol.ImageUri2));
                    temp.Id = lol.Id;
                    temp.Price = lol.Price.ToString();
                    temp.desc = lol.Description;
                    StoreList.Add(temp);
                }

                items2 = await Table2.ToCollectionAsync();
                foreach (Collections lol2 in items2)
                {
                    // temp = new StoreListing();
                    CollNames.Add(lol2.Name);
                }


                Box.AutoCompleteSource = BookNames;
                Box2.AutoCompleteSource = CollNames;

                StoreListView.ItemsSource = StoreList;
                StoreListView2.ItemsSource = items2;


                Box.Visibility = Visibility.Visible;
                StoreListView.Visibility = Visibility.Visible;
                SearchButton.Visibility = Visibility.Visible;
                LoadingBar.Visibility = Visibility.Collapsed;

                Box2.Visibility = Visibility.Visible;
                StoreListView2.Visibility = Visibility.Visible;
                SearchButton2.Visibility = Visibility.Visible;
                LoadingBar2.Visibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right can't go forward now");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StoreListView.Visibility = Visibility.Collapsed;
            SearchButton.Visibility = Visibility.Collapsed;
            LoadingBar.Visibility = Visibility.Visible;
            StoreList = new List<StoreListing>();
            LoadingBar.IsIndeterminate = true;
            StoreListing temp;
            try
            {
                items = await Table.Where(Book
                        => Book.Title.Contains(Box.Text) && Book.IsReady == true).ToCollectionAsync();
                foreach (Book lol in items)
                {
                    temp = new StoreListing();
                    temp.Id = lol.Id;
                    temp.Title = lol.Title;
                    temp.Author = lol.Author;
                    temp.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(lol.ImageUri2));
                    temp.Price = lol.Price.ToString();
                    temp.desc = lol.Description;
                    StoreList.Add(temp);
                }

                StoreListView.ItemsSource = StoreList;
                Box.Visibility = Visibility.Visible;
                StoreListView.Visibility = Visibility.Visible;
                SearchButton.Visibility = Visibility.Visible;
                LoadingBar.Visibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right at this time");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }


        private void StoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            StoreListing sent = e.ClickedItem as StoreListing;
            //sent.Price = 50.ToString();
            Frame.Navigate(typeof(StoreDetail), sent);
        }

        private async void SearchButton2_Click(object sender, RoutedEventArgs e)
        {
            StoreListView2.Visibility = Visibility.Collapsed;
            SearchButton2.Visibility = Visibility.Collapsed;
            LoadingBar2.Visibility = Visibility.Visible;

            LoadingBar.IsIndeterminate = true;
           
            try
            {
                items2 = await Table2.Where(Collections
                        => Collections.Name.Contains(Box2.Text)).ToCollectionAsync();


                StoreListView2.ItemsSource = items2;
                Box2.Visibility = Visibility.Visible;
                StoreListView2.Visibility = Visibility.Visible;
                SearchButton2.Visibility = Visibility.Visible;
                LoadingBar2.Visibility = Visibility.Collapsed;
                //write code to navigate to detail page of collection
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right at this time");
                await msgbox.ShowAsync();
                LoadingBar2.Visibility = Visibility.Collapsed;
            }
        }

        private void StoreListView2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Collections sent = e.ClickedItem as Collections;
            //sent.Price = 50.ToString();
            Frame.Navigate(typeof(CollDetail), sent);
        }
    }
}
