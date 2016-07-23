using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class CollDetail : Page
    {
        public CollDetail()
        {
            this.InitializeComponent();
        }
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;
        int Price=0;
        private IMobileServiceTable<Author> Table4 = App.MobileService.GetTable<Author>();
        private MobileServiceCollection<Author, Author> items4;
        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        private IMobileServiceTable<User> Table3 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items3;
        Collections rec;
        string test;
        List<string> book;
        List<string> bookName;
        List<string> ChapPur;
        List<string> Chap;
        List<string> lis;
        List<string> ChapName;
        string testlol = "";
        List<CollView> CollList;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

            items3 = await Table3.Where(User
                             => User.username == testlol).ToCollectionAsync();
            test = items3[0].purchases;

            CollList = new List<CollView>();
            book = new List<string>();
            Chap = new List<string>();

            bookName = new List<string>();
            ChapName = new List<string>();
            ChapPur = new List<string>();
            LoadingBar.IsIndeterminate = true;
            LoadingBar.Visibility = Visibility.Visible;
            rec = new Collections();
            rec = e.Parameter as Collections;
            Title.Text = rec.Name;
            Author.Text = rec.CreatedBy;
            FullCost.Text = "Full Collection Price: ";
            string[] lis = rec.books.Split(',');
            for(int i=0;i<lis.Length;i++)
            {
                string[] temp = lis[i].Split('.');
                book.Insert(i, temp[0]);
                Chap.Insert(i, temp[1]);
            }
            foreach (string lol in book)
            {
                    items2 = await Table2.Where(Book
                                       => Book.Id == lol).ToCollectionAsync();
                    bookName.Insert(bookName.Count, items2[0].Title);
            }
            foreach (string lol in Chap)
            {
                items = await Table.Where(Chapter
                    => Chapter.Id == lol).ToCollectionAsync();
                ChapName.Insert(ChapName.Count, items[0].Name);

                if (!test.Contains(lol))        //to check if chapter is already purchased or not 
                {
                    if (!test.Contains(items[0].bookid + ".full"))
                    {
                        ChapPur.Insert(ChapPur.Count, items[0].Id);
                        Price += items[0].price;
                    }
                }
            }
            FullCost.Text += Price.ToString();
            for (int i = 0; i < ChapName.Count; i++)
            {
                CollView temp = new CollView();
                temp.Book = bookName[i];
                temp.Chapter = ChapName[i];
                CollList.Insert(CollList.Count, temp);
            }
            StoreListView.ItemsSource = CollList;
            LoadingBar.Visibility = Visibility.Collapsed;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
          
            LoadingBar.IsEnabled = true;
            LoadingBar.Visibility = Visibility.Visible;
            App.mc.Clear();
            await Check_Exist();

        }

        private async Task Check_Exist()
        {
            try
            {
                items3 = await Table3.Where(User
                                  => User.username == testlol).ToCollectionAsync();
                User a = items3[0];
                if (ChapPur.Count != 0)//
                {
                    if (a.wallet > Price)
                    {
                        foreach (string lol in ChapPur)//buying the chapters
                        {
                            items = await Table.Where(Chapter
                              => Chapter.Id == lol).ToCollectionAsync();


                            a.purchases += items[0].bookid + "." + lol + ",";
                            a.wallet = a.wallet - items[0].price;
                            await Table3.UpdateAsync(a);
                            items2 = await Table2.Where(Book
                                   => Book.Id == items[0].bookid).ToCollectionAsync();

                            items4 = await Table4.Where(Author
                                   => Author.Id == items2[0].PublisherId).ToCollectionAsync();
                            Author c = items4[0];
                            c.wallet += items[0].price;
                            await Table4.UpdateAsync(c);
                            items = await Table.Where(Chapter
                                        => Chapter.Id == lol).ToCollectionAsync();
                            Chapter b = items[0];
                            b.downloads++;
                            await Table.UpdateAsync(b);
                        }
                        a.collections += rec.Id + ",";//adding collection to user list
                        await Table3.UpdateAsync(a);                     
                        LoadingBar.Visibility = Visibility.Collapsed;
                        MessageDialog mess = new MessageDialog("Purchase successfull! Download the file from My purchase section");
                        await mess.ShowAsync();
                        Frame.Navigate(typeof(Purchased));
                    }
                    else
                    {
                        LoadingBar.Visibility = Visibility.Collapsed;
                        MessageDialog mess1 = new Windows.UI.Popups.MessageDialog("You have insufficient funds for this!");
                        await mess1.ShowAsync();
                    }
                                       
                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog mess = new Windows.UI.Popups.MessageDialog("You have already purchased this Please make collection from your Chapters!");
                    await mess.ShowAsync();
                }
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Something is not right try againg later");
                await msgbox.ShowAsync();
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

    }
}
