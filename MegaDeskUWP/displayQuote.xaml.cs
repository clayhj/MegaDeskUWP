using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MegaDeskUWP
{

    public sealed partial class DisplayQuote : Page
    {
        public DisplayQuote()
        {
            InitializeComponent();
        }

        private void displayOK_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
