﻿using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using Newtonsoft.Json;
using Syncfusion.Pdf;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Heist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Downloads : Page
    {
        public Downloads()
        {
            this.InitializeComponent();
            lol();
        }
        public BitmapImage Im { get; set; }
        
        string testlol;
        BookData ob = new BookData();
        StorageFolder openBook = null;
        List<CollJson> obList = new List<CollJson>();
        EncryptionClass eob = new EncryptionClass();

        async void lol()
        {
            LoadingBar.IsActive = true;
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            await loadColl();
            await load();
            LoadingBar.Visibility = Visibility.Collapsed;

        }
        async Task retreive(string name)
        {

            name = eob.Not_For_This(name);
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
                    StorageFolder mainFol = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);

                    if (mainFol != null)
                    {
                        ob = new BookData();
                        StorageFolder folder = await mainFol.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
                        openBook = folder;

                        StorageFile sampleFile = await folder.GetFileAsync("UserName.txt");
                        var t = await sampleFile.OpenAsync(FileAccessMode.Read);
                        Stream na = t.AsStreamForRead();
                        using (var streamReader = new StreamReader(na, Encoding.UTF8))
                        {
                            string line;
                            line = streamReader.ReadToEnd();
                            ob = JsonConvert.DeserializeObject<BookData>(line);
                        }
                        IReadOnlyList<StorageFile> sf = await folder.GetFilesAsync();
                        StorageFile imgFile = await folder.GetFileAsync("image.jpeg");
                        Im = new BitmapImage(new Uri(imgFile.Path));

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
            catch(Exception)
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
            await(new MessageDialog("You are successfully loged out :):)")).ShowAsync();
            Frame.Navigate(typeof(Login));
        }

        private async Task load()
        {
            try
            {
                List<GridClass> lg = new List<GridClass>();
                GridClass gd = new GridClass();
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);
                IReadOnlyList<StorageFolder> sf = await folder.GetFoldersAsync();        
                gd.Image = new BitmapImage(new Uri(this.BaseUri ,"Assets/whataboutme.jpg"));
                gd.authName = "test me";
                gd.title = "about me";
                lg.Add(gd);
                foreach (StorageFolder s in sf)
                {
                    gd = new GridClass();
                    ob = new BookData();
                    StorageFile sampleFile = await s.GetFileAsync("UserName.txt");
                    var t = await sampleFile.OpenAsync(FileAccessMode.Read);
                    Stream na = t.AsStreamForRead();
                    using (var streamReader = new StreamReader(na, Encoding.UTF8))
                    {
                        string line;
                        line = streamReader.ReadToEnd();
                        ob = JsonConvert.DeserializeObject<BookData>(line);
                    }

                    IReadOnlyList<StorageFile> fi = await s.GetFilesAsync();
                    StorageFile imgFile = await s.GetFileAsync("image.jpeg");
                    Im = new BitmapImage(new Uri(imgFile.Path));
                    gd = new GridClass();
                    gd.title = ob.Title;
                    gd.Image = Im;
                    gd.authName = ob.Author;
                    lg.Add(gd);
                }

                if (lg.Count != 0)
                {
                    event1.ItemsSource = lg;

                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    ErrorBox.Text = "No Books Downloaded";
                    ErrorBox.Visibility = Visibility.Visible;
                }
            }
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Oops Something Bad Happened :(:(")).ShowAsync();
            }
        }

        private async Task loadColl()
        {
            CollJson ob;
            try
            {
                IList<string> sL = new List<string>();
                StorageFolder mainFol1 = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);
                StorageFile useFile1 = await mainFol1.CreateFileAsync("Collections.txt", CreationCollisionOption.OpenIfExists);
                sL = await FileIO.ReadLinesAsync(useFile1);
                List<GridClass> lg = new List<GridClass>();
                foreach (string ln in sL)
                {
                    ob = JsonConvert.DeserializeObject<CollJson>(ln);
                    obList.Add(ob);
                    GridClass gd = new GridClass();
                    gd = new GridClass();
                    gd.title = ob.name;
                    gd.authName = "";
                    gd.Image = new BitmapImage(new Uri(this.BaseUri, "Assets/BooksCollections.png"));
                    lg.Add(gd);
                }
                if (lg.Count != 0)
                {
                    event11.ItemsSource = lg;
                }
                else
                {
                    LoadingBar1.Visibility = Visibility.Collapsed;
                    ErrorBox1.Text = "No Collections Downloaded";
                    ErrorBox1.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                LoadingBar1.Visibility = Visibility.Collapsed;
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
                    Border b = child as Border;

                    titl = b.Child as FrameworkElement;
                }


                if ((Grid.GetRow(child) == 1) && (Grid.GetColumn(child) == 1))
                {
                    Border b = child as Border;

                    auth = b.Child as FrameworkElement;
                }
            }
            TextBlock t = titl as TextBlock;
            TextBlock t2 = auth as TextBlock;
            
            string str = t2.Text;

            if (str != "")
                await retreive(t.Text);
            else if (t.Text.CompareTo("about me") == 0)
            {
                await printPdf("ms-appx://Assets/test.pdf");
                event2.Visibility = Visibility.Collapsed;
                PdfGrid.Visibility = Visibility.Visible;
                Appbar.Visibility = Visibility.Visible;
            }
            else
            {
                string nam = t.Text;
                await printPdf(nam + ".txt");
                event2.Visibility = Visibility.Collapsed;
                PdfGrid.Visibility = Visibility.Visible;
                Appbar.Visibility = Visibility.Visible;
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
                    StorageFile stream = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.BaseUri , "Assets/lol.txt"));
                    Stream fileStream = await stream.OpenStreamForReadAsync();
                    pdfViewer.LoadDocument(fileStream);
                    event2.Visibility = Visibility.Collapsed;
                    PdfGrid.Visibility = Visibility.Visible;
                    Appbar.Visibility = Visibility.Visible;
                    TitlBox.Text = "About Me";
                    return;
                }
                ob.userName = ob.userName.ToUpper();
                
              if((ob.userName.CompareTo(testlol.ToUpper()) != 0))
                 {
                    await (new MessageDialog("Maybe this Pdf doesn't belong to you.If it does then download it again plzzz :):)")).ShowAsync();
                    Frame.Navigate(typeof(Downloads));
                 }
                StorageFile file = await openBook.GetFileAsync(text);
               string StDec = await FileIO.ReadTextAsync(file);

                //var l = await file.OpenAsync(FileAccessMode.Read);
                //Stream str = l.AsStreamForRead();
                //byte[] buffer = new byte[str.Length];
                //str.Read(buffer, 0, buffer.Length);

                byte[] buffer = eob.AES_Decrypt(StDec);

                // Loads the PDF document.
               
                PdfLoadedDocument ldoc = new PdfLoadedDocument(buffer);
                TitlBox.Text = "Chapter: " + text.Replace(".txt", "");
                pdfViewer.LoadDocument(ldoc);
                pdfViewer1.LoadDocument(ldoc);
               
               
            }
            catch(Exception e)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't open Pdf")).ShowAsync();
                Frame.Navigate(typeof(Downloads));
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
                ob.userName = ob.userName.ToUpper();
                if ((ob.userName.CompareTo(testlol.ToUpper()) != 0))
                {
                    await (new MessageDialog("Maybe this Pdf doesn't belong to you.If it does then download it again plzzz :):)")).ShowAsync();
                    Frame.Navigate(typeof(Downloads));
                }

                StorageFile file = await openBook.GetFileAsync(text);
                string StDec = await FileIO.ReadTextAsync(file);
                

                buffer = eob.AES_Decrypt(StDec);

                //var l = await file.OpenAsync(FileAccessMode.Read);
                //Stream str = l.AsStreamForRead();
                //buffer = new byte[str.Length];
                //str.Read(buffer, 0, buffer.Length);
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
              
                LoadingBarPdf.Visibility = Visibility.Collapsed;
                LoadingBarPdf1.Visibility = Visibility.Collapsed;
                mediaElement.Play();

            }
            catch(Exception)
            {
                LoadingBarPdf.Visibility = Visibility.Collapsed;
                LoadingBarPdf1.Visibility = Visibility.Collapsed;
                await (new MessageDialog("Can't read Pdf")).ShowAsync();
                Frame.Navigate(typeof(Downloads));
            }
        }

        private async void PlayPdf_Click(object sender, RoutedEventArgs e)
        {
            LoadingBarPdf.IsActive = true;
            LoadingBarPdf.Visibility = Visibility.Visible;
            LoadingBarPdf1.IsActive = true;
            LoadingBarPdf1.Visibility = Visibility.Visible;
            await tts(loc);

        }

        private async void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Grid g = new Grid();
            g = sender as Grid;

            FrameworkElement titl = null;
            FrameworkElement auth = null;
            foreach (FrameworkElement child in g.Children)
            {
                if ((Grid.GetRow(child) == 0) && (Grid.GetColumn(child) == 1))
                {
                    Border b = child as Border;

                    titl = b.Child as FrameworkElement;
                }

                if ((Grid.GetRow(child) == 1) && (Grid.GetColumn(child) == 1))
                {
                    Border b = child as Border;

                    auth = b.Child as FrameworkElement;
                }
            }
                TextBlock t = auth as TextBlock;
                TextBlock t1 = titl as TextBlock;
            StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(testlol + "My Books", CreationCollisionOption.OpenIfExists);
            if (t.Text == "")
              {
                string str = t1.Text;
                List<GridClass> lg = new List<GridClass>();
                GridClass gd = new GridClass();              
                foreach (CollJson o in obList)
                {
                    if (o.name == str)
                    {
                        for (int i = 0; i < o.list.Count; i++)
                        {
                            string na = o.list[i].Item1;
                            StorageFolder folde = await folder.GetFolderAsync(eob.Not_For_This(na));
                            StorageFile imgFile = await folde.GetFileAsync("image.jpeg");
                            Im = new BitmapImage(new Uri(imgFile.Path));
                            gd = new GridClass();
                            gd.title = na;
                            gd.Image = Im;
                            gd.authName = o.list[i].Item2;
                            lg.Add(gd);
                        }
                    }
                }
                event11.ItemsSource = lg;
            }
            else
            {
                openBook = await folder.GetFolderAsync(eob.Not_For_This(t1.Text));
                await printPdf(eob.Not_For_This(t.Text) + ".txt");
                event21.Visibility = Visibility.Collapsed;
                event11.Visibility = Visibility.Collapsed;
                PdfGrid1.Visibility = Visibility.Visible;
                Appbar.Visibility = Visibility.Visible;
            }
        }
            
    }
}
