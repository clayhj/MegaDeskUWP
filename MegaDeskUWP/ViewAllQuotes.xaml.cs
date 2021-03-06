﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDeskUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewAllQuotes : Page
    {
        public ViewAllQuotes()
        {
            this.InitializeComponent();
        }

        private async void viewQuotesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile quoteFile = await storageFolder.GetFileAsync("quotes.json");
                string readQuotes = await FileIO.ReadTextAsync(quoteFile);


                var quotes = JsonConvert.DeserializeObject<List<DeskQuote>>(readQuotes);

                allQuotes.ItemsSource = quotes.Select(d => new
                {
                    d.QuoteDate,
                    d.CustomerName,
                    d.Desk.Depth,
                    d.Desk.Width,
                    d.Desk.Drawers,
                    d.Desk.DeskMaterial,
                    d.RushOrder,
                    d.QuotePrice

                }).ToList();
            }
            catch (FileNotFoundException)
            {
                var msg = new MessageDialog("There are no quotes");
                await msg.ShowAsync();
            }
        }
            

        private void mainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void deleteQuotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile quoteFile = await storageFolder.GetFileAsync("quotes.json");
                await quoteFile.DeleteAsync(StorageDeleteOption.Default);
                this.Frame.Navigate(GetType());
            }
            catch (FileNotFoundException)
            {
                var msg = new MessageDialog("There are no quotes to delete");
                await msg.ShowAsync();
            }
        }
    }
}
