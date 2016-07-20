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
    public sealed partial class PurchasedCollDetail : Page
    {

        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;
        private IMobileServiceTable<User> Table3 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items3;
        string test;
        List<string> book;
        List<string> bookName;
        List<string> ChapPur;
        List<string> Chap;
        List<string> lis;
        List<string> ChapName;
        MeraCollView rec;
        string testlol = "";
        private List<ChapterView> list;
        List<CollView> CollList;
        public byte[] imgBuffer;

        public PurchasedCollDetail()
        {
            this.InitializeComponent();
        }


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
            rec = new MeraCollView();
            rec = e.Parameter as MeraCollView;
            Cover.Source = rec.sel.Image;
            Title.Text = rec.sel.Title;
            Author.Text = rec.sel.Author;
            string[] lis = rec.purchases.Split(',');
            for (int i = 0; i < lis.Length; i++)
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

    }

    private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            string sn = "";
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;

            items2 = await Table2.Where(Book
                            => Book.Id == rec.Id).ToCollectionAsync();
            BookData b = new BookData();
            foreach (Book lol in items2)
            {
                b.Title = lol.Title;
                b.Author = lol.Author;
                b.userName = testlol;

                try
                {
                    HttpClient client = new HttpClient(); // Create HttpClient
                    imgBuffer = await client.GetByteArrayAsync(lol.ImageUri2); // Download file

                    sn = JsonConvert.SerializeObject(b);

                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    await (new MessageDialog("Something bad happened :(:(")).ShowAsync();
                    break;
                }
            }
            try
            {
                var test = sender as Button;
                var test2 = test.Parent as Grid;
                var test3 = test2.Children[2] as TextBlock;
                var test4 = test2.Children[0] as TextBlock;
                string nam = test4.Text;

                string titl = Title.Text;
                Uri url = new Uri("https://ebookstreamer.me/downloads");
                HttpClient httpClient = new HttpClient();
                var myClientHandler = new HttpClientHandler();

                //myClientHandler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var content = new FormUrlEncodedContent(new[]
                 {
                new KeyValuePair<string, string>("id", test3.Text)
            });
                httpResponse = await httpClient.PostAsync(url, content);
                httpResponse.EnsureSuccessStatusCode();
                Stream str = await httpResponse.Content.ReadAsStreamAsync();

                byte[] pd = new byte[str.Length];
                str.Read(pd, 0, pd.Length);
                try
                {
                    StorageFolder mainFol = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);
                    if (mainFol != null)
                    {
                        StorageFolder folder = await mainFol.CreateFolderAsync(titl, CreationCollisionOption.OpenIfExists);
                        if (folder != null)
                        {
                            StorageFile file = await folder.CreateFileAsync(nam + ".txt", CreationCollisionOption.ReplaceExisting);
                            using (var fileStream = await file.OpenStreamForWriteAsync())
                            {
                                str.Seek(0, SeekOrigin.Begin);
                                await str.CopyToAsync(fileStream);
                            }
                            StorageFile useFile =
                           await folder.CreateFileAsync("UserName.txt", CreationCollisionOption.ReplaceExisting);
                            await Windows.Storage.FileIO.WriteTextAsync(useFile, sn);
                            StorageFile imgFile =
                         await folder.CreateFileAsync("image.jpeg", CreationCollisionOption.ReplaceExisting);
                            using (Stream stream = await imgFile.OpenStreamForWriteAsync())
                                stream.Write(imgBuffer, 0, imgBuffer.Length); // Save
                        }
                    }
                    LoadingBar.Visibility = Visibility.Collapsed;
                    await (new MessageDialog("Download Successful")).ShowAsync();
                    Frame.Navigate(typeof(Downloads));
                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    await (new MessageDialog("Can't download now please try after sometime")).ShowAsync();
                }
            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Something bad happened :(:(")).ShowAsync();
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
    }
}
