using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDeskUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisplayQuote : Page
    {
        private DeskQuote _deskQuote;

        public DisplayQuote()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DeskQuote deskQuote = e.Parameter as DeskQuote;
            customerName.Text = deskQuote.CustomerName;
            deskSize.Text = deskQuote.DeskSize().ToString();
            drawers.Text = deskQuote.Desk.Drawers.ToString();
            deskMaterial.Text = deskQuote.Desk.DeskMaterial;
            rushOrder.Text = deskQuote.RushOrder.ToString() + " Day";
            totalPrice.Text = string.Format("{0:C}", deskQuote.QuotePrice);
            _deskQuote = deskQuote;
        }

        private async void saveQuoteButton_Click(object sender, RoutedEventArgs e)
        {
            string newQuotes;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFile storageFile = await storageFolder.GetFileAsync("quotes.json");
                string readQuotes = await FileIO.ReadTextAsync(storageFile);
                var quotes = JsonConvert.DeserializeObject<List<DeskQuote>>(readQuotes);
                quotes.Add(_deskQuote);
                newQuotes = JsonConvert.SerializeObject(quotes);
                await FileIO.WriteTextAsync(storageFile, newQuotes);
            }
            catch (FileNotFoundException)
            {
                StorageFile storageFile = await storageFolder.CreateFileAsync("quotes.json", CreationCollisionOption.ReplaceExisting);
                var deskQuotes = new List<DeskQuote> { _deskQuote };
                var myDeskQuote = JsonConvert.SerializeObject(deskQuotes);
                await FileIO.WriteTextAsync(storageFile, myDeskQuote);
            }

            saveQuoteButton.IsEnabled = false;

        }

        private void mainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
