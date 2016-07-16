using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class StoreDetail : Page
    {
        private IMobileServiceTable<User> Table2 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items2;
        private IMobileServiceTable<Author> Table3 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items3;
        private StoreListing rec;
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;
        private IMobileServiceTable<Book> Table4 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items4;


        string testlol;
        private List<ChapterView> list;
        public StoreDetail()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var a = Window.Current.Bounds;
            if (a.Width < 720)
            {
                
                var col = new ColumnDefinition();
                col.Width = new GridLength(a.Width - 200);
                descBoxGrid.ColumnDefinitions.Add(col);
                col = new ColumnDefinition();
                col.Width = new GridLength(200);
                descBoxGrid.ColumnDefinitions.Add(col);
                descBoxGrid.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                var col = new ColumnDefinition();
                col.Width = new GridLength(a.Width - 200);
                descBoxGrid.ColumnDefinitions.Add(col);
                col = new ColumnDefinition();
                col.Width = new GridLength(200);
                //descBoxGrid.ColumnDefinitions.Add(col);
                //descBoxGrid.Width = a.Width / 2;
                descBoxGrid.HorizontalAlignment = HorizontalAlignment.Center;
                //StoreListView.Width = a.Width - descBoxGrid.Width;
            }
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadingBar.IsIndeterminate = true;
            LoadingBar.Visibility = Visibility.Visible;
            rec = new StoreListing();
            rec = e.Parameter as StoreListing;
            Title.Text = rec.Title;
            Cover.Source = rec.Image;
            FullCost.Text = rec.Price;
            Author.Text = rec.Author;
            DescBlock.Text = rec.desc;
            FullCost.Text = "Full Book Price: " + rec.Price;

            try
            {
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await folder.GetFileAsync("sample.txt");

                testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                list = new List<ChapterView>();
                ChapterView temp;
                try
                {
                    items = await Table.Where(Chapter
                                => Chapter.bookid == rec.Id).ToCollectionAsync();

                    foreach (Chapter lol in items)
                    {
                        temp = new ChapterView();
                        temp.Id = lol.Id;
                        temp.Title = lol.Name;
                        temp.Price = "Price: " + lol.price.ToString();
                        list.Add(temp);
                    }
                    LoadingBar.Visibility = Visibility.Collapsed;
                    StoreListView.ItemsSource = list;
                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog mess = new Windows.UI.Popups.MessageDialog("Sorry Can't load the chapters now :(:(");
                    await mess.ShowAsync();
                }
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right try later");
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

        private  void MenuButton4_Click(object sender, RoutedEventArgs e)
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

        private async void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("You are successfully loged out :):)")).ShowAsync();
            Frame.Navigate(typeof(Login));
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.IsIndeterminate = true;
            LoadingBar.Visibility = Visibility.Visible;

            try
            {
                //TODO Download full book and add entry to user purchases
                items2 = await Table2.Where(User
                                => User.username == testlol).ToCollectionAsync();
                User a = items2[0];
                if (!a.purchases.Contains(rec.Id + ".full"))
                {
                    if (a.wallet > int.Parse(rec.Price))
                    {
                        a.purchases += rec.Id + ".full,";
                        a.wallet = a.wallet - int.Parse(rec.Price);
                        await Table2.UpdateAsync(a);

                        items4 = await Table4.Where(Book
                                  => Book.Id == rec.Id).ToCollectionAsync();
                        items3 = await Table3.Where(Author
                               => Author.Id == items4[0].PublisherId).ToCollectionAsync();
                        Author b = items3[0];
                        b.wallet += int.Parse(rec.Price);
                        await Table3.UpdateAsync(b);
                        LoadingBar.Visibility = Visibility.Collapsed;
                        Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("Purchase successfull! Download the file from My purchase section");
                        await mess.ShowAsync();
                    }
                    else
                    {
                        LoadingBar.Visibility = Visibility.Collapsed;
                        Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("You have insufficient funds for this!");
                        await mess.ShowAsync();
                    }
                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("You have already purchased this!");
                    await mess.ShowAsync();
                }
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right try againg later");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {

            //take rec.id to send in post with header id
            try
            {
                LoadingBar.IsIndeterminate = true;
                LoadingBar.Visibility = Visibility.Visible;
                var test = sender as Button;
                var test2 = test.Parent as Grid;
                var test3 = test2.Children[3] as TextBlock;
                var test5 = test2.Children[1] as TextBlock;
                string hello = test5.Text.Substring(7);
                items2 = await Table2.Where(User
                               => User.username == testlol).ToCollectionAsync();
                User a = items2[0];
                if (!a.purchases.Contains(rec.Id + "." + test3.Text) && !a.purchases.Contains(rec.Id + ".full"))
                {
                    if (a.wallet > int.Parse(hello))
                    {
                        a.purchases += rec.Id + "." + test3.Text + ",";
                        a.wallet = a.wallet - int.Parse(hello);
                        await Table2.UpdateAsync(a);
                        items4 = await Table4.Where(Book
                                   => Book.Id == rec.Id).ToCollectionAsync();
                        items3 = await Table3.Where(Author
                               => Author.Id == items4[0].PublisherId).ToCollectionAsync();
                        Author c = items3[0];
                        c.wallet += int.Parse(hello);
                        await Table3.UpdateAsync(c);
                        items = await Table.Where(Chapter
                                    => Chapter.Id == test3.Text).ToCollectionAsync();
                        Chapter b = items[0];
                        b.downloads++;
                        await Table.UpdateAsync(b);
                        LoadingBar.Visibility = Visibility.Collapsed;
                        Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("Purchase successfull! Download the file from My purchase section");
                        await mess.ShowAsync();
                        Frame.Navigate(typeof(Purchased));
                    }
                    else {
                        LoadingBar.Visibility = Visibility.Collapsed;
                        Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("You have insufficient funds for this!");
                        await mess.ShowAsync();
                    }
                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    Windows.UI.Popups.MessageDialog mess = new Windows.UI.Popups.MessageDialog("You have already purchased this!");
                    await mess.ShowAsync();
                }
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right try again");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
