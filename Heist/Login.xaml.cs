using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
    public sealed partial class Login : Page
    {
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        public Login()
        {
            this.InitializeComponent();
        }

        private async void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                await lol();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await lol();
        }
        private async Task lol()
        { 
            OfflineCheck oc = new OfflineCheck();
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            try
            {
                items = await Table.Where(User
                                => User.username == UserName.Text).ToCollectionAsync();
                if (items.Count != 0)
                {
                    if (Password.Password == items[0].password)
                    {
                        oc.userName = UserName.Text;
                        oc.password = Password.Password;
                        LoadingBar.Visibility = Visibility.Collapsed;
                        MessageDialog msgbox = new MessageDialog("Welcome " + UserName.Text);
                        await msgbox.ShowAsync();
                        StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                        StorageFile sampleFile =
                            await folder.CreateFileAsync("sample.txt", CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(sampleFile, UserName.Text);

                        StorageFile notNet =
                            await folder.CreateFileAsync("check.txt", CreationCollisionOption.ReplaceExisting);
                        string sn = JsonConvert.SerializeObject(oc);
                        await Windows.Storage.FileIO.WriteTextAsync(notNet, sn);

                        Frame.Navigate(typeof(Downloads));
                    }
                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Password or username incorrect");
                    await msgbox.ShowAsync();
                }
            }
            catch (Exception)
            {
                try
                {
                    OfflineCheck o;
                    StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    StorageFile sampleFile = await folder.GetFileAsync("check.txt");
                    var t = await sampleFile.OpenAsync(FileAccessMode.Read);
                    Stream na = t.AsStreamForRead();
                    using (var streamReader = new StreamReader(na, Encoding.UTF8))
                    {
                        string line;
                        line = streamReader.ReadToEnd();
                        o = JsonConvert.DeserializeObject<OfflineCheck>(line);
                        if ((UserName.Text.CompareTo(o.userName) == 0) && (Password.Password.CompareTo(o.password) == 0))
                        {
                            LoadingBar.Visibility = Visibility.Collapsed;
                            MessageDialog msgbo = new MessageDialog("Welcome " + UserName.Text);
                            await msgbo.ShowAsync();
                            msgbo = new MessageDialog("You are not connected online hence you could only read the downloaded books..");
                            await msgbo.ShowAsync();
                            Frame.Navigate(typeof(Downloads));
                        }
                    }
                }
                catch (Exception)
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Sorry Can't connect");
                    await msgbox.ShowAsync();
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }

        private void Button_Click_2(System.Object sender, RoutedEventArgs e)
        {

        }
    }
}
