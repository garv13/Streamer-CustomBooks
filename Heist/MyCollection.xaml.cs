using Newtonsoft.Json;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
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
    public sealed partial class MyCollection : Page
    {
        public MyCollection()
        {
            App.mc.Clear();
            this.InitializeComponent();
            lol();
        }


        public BitmapImage Im { get; set; }
        string testlol;
        MeriCollection ob = new MeriCollection();
        StorageFolder openBook = null;

        async void lol()
        {
            LoadingBar.IsActive = true;
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            await load();
            LoadingBar.Visibility = Visibility.Collapsed;

        }
        async Task retreive(string name)
        {

            try
            {
                List<GridClass> lg = new List<GridClass>();
                GridClass gd = new GridClass();

                if (name.CompareTo("about me") == 0)
                {
                    gd.Image = new BitmapImage(new Uri(this.BaseUri, "Assets/whataboutme.jpg"));
                    gd.authName = "";
                    gd.title = "about me";
                    lg.Add(gd);
                    event1.Visibility = Visibility.Collapsed;
                    event2.ItemsSource = lg;
                    event2.Visibility = Visibility.Visible;
                }
                else
                {
                    StorageFolder mainFol = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Collections", CreationCollisionOption.OpenIfExists);

                    if (mainFol != null)
                    {
                        ob = new MeriCollection();
                        StorageFolder folder = await mainFol.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
                        openBook = folder;

                        StorageFile sampleFile = await folder.GetFileAsync("UserName.txt");
                        var t = await sampleFile.OpenAsync(FileAccessMode.Read);
                        Stream na = t.AsStreamForRead();
                        using (var streamReader = new StreamReader(na, Encoding.UTF8))
                        {
                            string line;
                            line = streamReader.ReadToEnd();
                            ob = JsonConvert.DeserializeObject<MeriCollection>(line);
                        }
                        IReadOnlyList<StorageFile> sf = await folder.GetFilesAsync();
                        foreach (StorageFile s in sf)
                        {
                            gd = new GridClass();
                            if (s.Name.CompareTo("UserName.txt") != 0)
                            {
                                if (s.Name.CompareTo("image.jpeg") != 0)
                                {
                                    gd = new GridClass();
                                    gd.title = s.DisplayName;
                                    gd.Image = Im;
                                    gd.authName = "";
                                    lg.Add(gd);
                                }
                            }
                        }
                        event1.Visibility = Visibility.Collapsed;
                        event2.ItemsSource = lg;
                        event2.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                await (new MessageDialog("ahhmm Something not so good Happened :(:(")).ShowAsync();
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

        private async Task load()
        {
            try
            {
                List<GridClass> lg = new List<GridClass>();
                GridClass gd = new GridClass();
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Collections", CreationCollisionOption.OpenIfExists);
                IReadOnlyList<StorageFolder> sf = await folder.GetFoldersAsync();
                gd.Image = new BitmapImage(new Uri(this.BaseUri, "Assets/BooksCollections.png"));
                gd.authName = "test me";
                gd.title = "about me";
                lg.Add(gd);
                foreach (StorageFolder s in sf)
                {
                    gd = new GridClass();
                    ob = new MeriCollection();
                    StorageFile sampleFile = await s.GetFileAsync("UserName.txt");
                    var t = await sampleFile.OpenAsync(FileAccessMode.Read);
                    Stream na = t.AsStreamForRead();
                    using (var streamReader = new StreamReader(na, Encoding.UTF8))
                    {
                        string line;
                        line = streamReader.ReadToEnd();
                        ob = JsonConvert.DeserializeObject<MeriCollection>(line);
                    }

                    IReadOnlyList<StorageFile> fi = await s.GetFilesAsync();
                    gd = new GridClass();
                    gd.title = ob.BookName;
                    gd.authName = ob.UserName;
                    lg.Add(gd);
                }

                if (lg.Count != 0)
                {
                    event1.ItemsSource = lg;

                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    ErrorBox.Text = "No Collections created";
                    ErrorBox.Visibility = Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Oops Something Bad Happened :(:(")).ShowAsync();
            }
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoadingBar.IsActive = true;
            LoadingBar.Visibility = Visibility.Visible;
            Grid g = new Grid();
            g = sender as Grid;
            FrameworkElement auth = null;
            FrameworkElement titl = null;
            foreach (FrameworkElement child in g.Children)
            {
                if ((Grid.GetRow(child) == 0) && (Grid.GetColumn(child) == 1))
                {
                    titl = child;
                }


                if ((Grid.GetRow(child) == 1) && (Grid.GetColumn(child) == 1))
                {
                    auth = child;
                }
            }
            TextBlock t = auth as TextBlock;
            TextBlock t2 = titl as TextBlock;

            string str = t.Text;

            if (str != "")
                await retreive(t2.Text);
            else if (t2.Text.CompareTo("about me") == 0)
                await printPdf("ms-appx://Assets/test.pdf");
            else
            {
                string nam = t2.Text;
                await printPdf(nam + ".txt");
            }
            LoadingBar.Visibility = Visibility.Collapsed;
        }

        string loc = null;

        private async Task printPdf(string text)
        {
            loc = text;
            try
            {
                if (text.CompareTo("ms-appx://Assets/test.pdf") == 0)
                {
                    StorageFile stream = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.BaseUri, "Assets/lol.txt"));
                    Stream fileStream = await stream.OpenStreamForReadAsync();
                    pdfViewer.LoadDocument(fileStream);
                    event2.Visibility = Visibility.Collapsed;
                    PdfGrid.Visibility = Visibility.Visible;
                    CreateColl.Visibility = Visibility.Collapsed;
                    PlayPdf.Visibility = Visibility.Visible;
                    Appbar.Visibility = Visibility.Visible;
                    TitlBox.Text = "About Me";
                    return;
                }
                if ((ob.UserName.CompareTo(testlol) != 0))
                {
                    await (new MessageDialog("Maybe this Pdf doesn't belong to you.If it does then download it again plzzz :):)")).ShowAsync();
                    Frame.Navigate(typeof(Downloads));
                }
                StorageFile file = await openBook.GetFileAsync(text);
                var l = await file.OpenAsync(FileAccessMode.Read);
                Stream str = l.AsStreamForRead();
                byte[] buffer = new byte[str.Length];
                str.Read(buffer, 0, buffer.Length);

                // Loads the PDF document.

                PdfLoadedDocument ldoc = new PdfLoadedDocument(buffer);
                TitlBox.Text = "Chapter: " + text.Replace(".txt", "");
                pdfViewer.LoadDocument(ldoc);
                event2.Visibility = Visibility.Collapsed;
                PdfGrid.Visibility = Visibility.Visible;
                CreateColl.Visibility = Visibility.Collapsed;
                PlayPdf.Visibility = Visibility.Visible;
                Appbar.Visibility = Visibility.Visible;
              


            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't open Pdf")).ShowAsync();
            }
        }

        async Task tts(string text)
        {
            byte[] buffer;
            try
            {
                if (text.CompareTo("ms-appx://Assets/test.pdf") == 0)
                {
                    StorageFile stream = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.BaseUri, "Assets/lol.txt"));
                    Stream fileStream = await stream.OpenStreamForReadAsync();
                    buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    goto p;
                }

                if ((ob.UserName.CompareTo(testlol) != 0))
                {
                    await (new MessageDialog("Maybe this Pdf doesn't belong to you.If it does then download it again plzzz :):)")).ShowAsync();
                    Frame.Navigate(typeof(Downloads));
                }

                StorageFile file = await openBook.GetFileAsync(text);
                var l = await file.OpenAsync(FileAccessMode.Read);
                Stream str = l.AsStreamForRead();
                buffer = new byte[str.Length];
                str.Read(buffer, 0, buffer.Length);
                p:;
                // Loads the PDF document.
                PdfLoadedDocument ldoc = new PdfLoadedDocument(buffer);
                // Loading Page collections
                PdfLoadedPageCollection loadedPages = ldoc.Pages;

                string s = "";

                // Extract text from PDF document pages
                foreach (PdfLoadedPage lpage in loadedPages)
                {
                    s += lpage.ExtractText();
                }

                s = s.Replace("\r", "");
                s = s.Replace("\n", "");
                SpeechSynthesizer synt = new SpeechSynthesizer();
                SpeechSynthesisStream syntStream = await synt.SynthesizeTextToStreamAsync(s);
                mediaElement.DefaultPlaybackRate = 0.85;
                mediaElement.SetSource(syntStream, syntStream.ContentType);
                mediaElement.Play();
                LoadingBarPdf.Visibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
                LoadingBarPdf.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't read Pdf")).ShowAsync();
            }
        }

        private async void PlayPdf_Click(object sender, RoutedEventArgs e)
        {
            LoadingBarPdf.IsActive = true;
            LoadingBarPdf.Visibility = Visibility.Visible;
            await tts(loc);

        }


   // replaced with My collection page navigation 

        private void CreateColl_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateCollection));
        }
    }
}
