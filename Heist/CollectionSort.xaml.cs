﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class CollectionSort : Page
    {
        public CollectionSort()
        {
            this.InitializeComponent();       
        }
        string testlol = "";
        string selected = "";
        ObservableCollection<CollSort> myList;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.GetFileAsync("sample.txt");
            testlol = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            myList = new ObservableCollection<CollSort>();
            foreach (MeriCollection d in App.mc)
            {
                CollSort c = new CollSort();
                c.BookId = d.BookId;
                c.BookName = d.BookName;
                c.ChapterId = d.ChapterId;
                c.ChapterNo = d.ChapterNo;
                c.UserName = d.UserName;
                c.sel = false;
                myList.Insert(0, c);
            }
            View.ItemsSource = myList;
            myList.CollectionChanged += MyList_CollectionChanged;
        }

        private void MyList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            View.ItemsSource = myList;
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
       

        private async void NextBar_Click(object sender, RoutedEventArgs e)
        {
            if(CollName.Text == "")
            {
                await (new MessageDialog("Enter Collection Name")).ShowAsync();
                return;
            }
            int i = 0;
            LoadingBar.Visibility = Visibility.Visible;
            App.mc.Clear();
            foreach (CollSort d in myList)
            {
                MeriCollection c = new MeriCollection();
                c.BookId = d.BookId;
                c.BookName = d.BookName;
                c.ChapterId = d.ChapterId;
                c.ChapterNo = d.ChapterNo;
                c.UserName = d.UserName;
                App.mc.Insert(i, c);
                i++;
            }
            LoadingBar.IsActive = true;     
                LoadingBar.Visibility = Visibility.Collapsed;
               
                Frame.Navigate(typeof(ShareColl), CollName.Text);

        }

        private async void BackBar_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Collapsed;
            await (new MessageDialog("Your collection was not made")).ShowAsync();
            Frame.Navigate(typeof(Purchased));
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            var test = sender as RadioButton;
            var test2 = test.Parent as Grid;
            var test3 = test2.Children[0] as TextBlock;
           
            var test4 = test2.Children[4] as TextBlock;
            selected = test4.Text;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bool hell = false;
            CollSort temp = new CollSort();
            foreach (CollSort l in myList)
            {
                if (l.ChapterId == selected)
                {
                    temp = l;
                    hell = true;
                    break;
                }
            }
            if (hell)
            {
                int i = myList.IndexOf(temp);
                if (i != 0)
                {
                    myList.Remove(temp);
                    i--;
                    myList.Insert(i, temp);
                }
                View.DataContext = myList;
            }
        }

        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            bool hell = false;
            CollSort temp = new CollSort();
            foreach (CollSort l in myList)
            {
                if (l.ChapterId == selected)
                {
                    temp = l;
                    hell = true;
                    break;
                }
            }
            if (hell)
            {
                int i = myList.IndexOf(temp);
                if (i != myList.Count - 1)
                {
                    myList.Remove(temp);
                    i++;
                    myList.Insert(i, temp);
                }
                View.DataContext = myList;
            }
        }
    }
}
