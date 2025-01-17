﻿using Microsoft.WindowsAzure.MobileServices;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Heist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareColl : Page
    {
        public ShareColl()
        {
            this.InitializeComponent();
        }

        private IMobileServiceTable<User> Table3 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items3;
        string testlol = "";
        string selected = "";
        string name="";
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            name = e.Parameter as string;
            Collections myList = new Collections();
            myList.CreatedBy = testlol;
            myList.downloads = 0;
            myList.like = 0;
            myList.books = "";
            myList.Name = name;
            foreach (MeriCollection lol in App.mc)
            {
                myList.books += lol.BookId + "." + lol.ChapterId + ",";
            }
            myList.books = myList.books.Substring(0, myList.books.Length - 1);
            await App.MobileService.GetTable<Collections>().InsertAsync(myList);
            items3 = await Table3.Where(User
                                 => User.username == testlol).ToCollectionAsync();
            User a = items3[0];
            a.collections += myList.Id + ",";//adding collection to user list
            await Table3.UpdateAsync(a);
            await (new MessageDialog("Your collection was made!!")).ShowAsync();
            Frame.Navigate(typeof(Purchased));
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
       // replaced with My collection page navigation 

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Purchased));
        }
    }
}
