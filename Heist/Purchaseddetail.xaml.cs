using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
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
    public sealed partial class Purchaseddetail : Page
    {
        private IMobileServiceTable<Book> Table2 = App.MobileService.GetTable<Book>();
        private MobileServiceCollection<Book, Book> items2;
        private PurchasedView rec;
        private IMobileServiceTable<Chapter> Table = App.MobileService.GetTable<Chapter>();
        private MobileServiceCollection<Chapter, Chapter> items;
        string testlol;
        private List<ChapterView> list;
        public byte[] imgBuffer; 
        public Purchaseddetail()
        {
            this.InitializeComponent();
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
                        else {
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
            
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't get data now please try again later")).ShowAsync();
            }
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            string sn = "";
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;

            items2 = await Table2.Where(Book
                            => Book.Id == rec.sel.Id).ToCollectionAsync();
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
                 
                //test code call
               // lo(test3.Text);
                // 
        
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
            catch(Exception)
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

        public async void lo(string st)
        {
            StringBuilder postString = new StringBuilder();
            postString.Append("&");
            postString.AppendFormat("{0}={1}", "id", "lololo");
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] byteArray = ascii.GetBytes(postString.ToString());

            // Create a request using a URL that can receive a post. 
            String MyURI = "https://posttestserver.com/post.php/";

            WebRequest request = WebRequest.Create(MyURI);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            // Get the request stream.
            Stream dataStream = await request.GetRequestStreamAsync();
            // Write the data to the request stream.
           
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.

            // Get the response.
            WebResponse response = await request.GetResponseAsync();
            // Display the status.
            string mes = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
           string res= (responseFromServer);
            // Clean up the streams.
            reader.Dispose();
            dataStream.Dispose();
            response.Dispose();



        }
    }
}
