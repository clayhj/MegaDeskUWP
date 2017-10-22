using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDeskUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddQuote : Page
    {
        public AddQuote()
        {
            this.InitializeComponent();
            deskMaterial.ItemsSource = Enum.GetValues(typeof(Desk.Material));
            deskMaterial.SelectedIndex = 0;
            Dictionary<string, int> rushDays = new Dictionary<string, int>();
            rushDays.Add("3 Days", 3);
            rushDays.Add("5 Days", 5);
            rushDays.Add("7 Days", 7);

            rushOrderDaysBox.ItemsSource = rushDays;
            rushOrderDaysBox.DisplayMemberPath = "Key";
            rushOrderDaysBox.SelectedIndex = 0;

        }

        private void rushOrderCheck_Click(object sender, RoutedEventArgs e)
        {
            if (rushOrderCheck.IsChecked == true)
            {
                rushOrderDaysBox.Visibility = Visibility.Visible;
            }
            else if (rushOrderCheck.IsChecked == false)
            {
                rushOrderDaysBox.Visibility = Visibility.Collapsed;
            }
        }

        private async void getQuoteButton_Click(object sender, RoutedEventArgs e)
        {
            string prices = "60,70,80,40,50,60,30,30,40";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync("rushOrderPrices.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, prices);
            string rushOrderPriceRaw = await FileIO.ReadTextAsync(storageFile);
            string[] rushOrderArray = rushOrderPriceRaw.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            customerName.Background = new SolidColorBrush(Colors.White);
            deskWidth.Background = new SolidColorBrush(Colors.White);
            deskDepth.Background = new SolidColorBrush(Colors.White);

            if (customerName.Text.Contains(',') || String.IsNullOrEmpty(customerName.Text))
            {
                customerName.Background = new SolidColorBrush(Colors.Red);
                var msg = new MessageDialog("Customer name must not be empty");
                await msg.ShowAsync();
            }
            else if (!Regex.IsMatch(deskWidth.Text, "^[0-9]*$"))
            {
                var msg = new MessageDialog("Width must only contain numbers.");
                deskWidth.Background = new SolidColorBrush(Colors.Red);
                await msg.ShowAsync();
            }
            else if (!Regex.IsMatch(deskDepth.Text, "^[0-9]*$"))
            {
                var msg = new MessageDialog("Depth must only contain numbers.");
                deskDepth.Background = new SolidColorBrush(Colors.Red);
                await msg.ShowAsync();
            }
            else
            {
                if (int.Parse(deskWidth.Text) < 24 || int.Parse(deskWidth.Text) > 96)
                {
                    var msg = new MessageDialog("Width must be between 24 and 96");
                    deskWidth.Background = new SolidColorBrush(Colors.Red);
                    await msg.ShowAsync();
                }
                else if (int.Parse(deskDepth.Text) < 12 || int.Parse(deskDepth.Text) > 48)
                {
                    var msg = new MessageDialog("Depth must be between 12 and 48");
                    deskDepth.Background = new SolidColorBrush(Colors.Red);
                    await msg.ShowAsync();
                }
                else
                {
                    Desk desk = new Desk();
                    desk.Width = int.Parse(deskWidth.Text);
                    desk.Depth = int.Parse(deskDepth.Text);
                    desk.Drawers = (int)numberOfDrawers.Value;
                    desk.DeskMaterial = deskMaterial.SelectedItem.ToString();
                    DeskQuote deskQuote = new DeskQuote(desk);
                    deskQuote.RushOrderPriceArray = rushOrderArray;
                    deskQuote.CustomerName = customerName.Text;
                    deskQuote.QuoteDate = DateTime.Now.ToString("MM/dd/yyyy");
                    deskQuote.QuotePrice = deskQuote.GetQuote();

                    if (rushOrderCheck.IsChecked == true)
                    {
                        deskQuote.RushOrder = ((KeyValuePair<string, int>)rushOrderDaysBox.SelectedValue).Value;
                    }
                    else
                    {
                        deskQuote.RushOrder = 14;
                    }

                    //Navigate to new page 
                    this.Frame.Navigate(typeof(DisplayQuote), deskQuote);
                }
            }
        }

        private void mainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void deskWidth_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!char.IsDigit((char)e.Key))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        } 

        private void deskDepth_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!char.IsDigit((char)e.Key))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void customerName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (Regex.IsMatch(e.Key.ToString(), "^[A-Za-z ]+$"))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
