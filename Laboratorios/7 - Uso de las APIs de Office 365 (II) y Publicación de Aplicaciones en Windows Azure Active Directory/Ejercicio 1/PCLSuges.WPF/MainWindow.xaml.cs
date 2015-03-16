using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PCLSuges.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWin32Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<MyContacts> results = new List<MyContacts>();
            SearchResults.ItemsSource = results;

        }

        private async void Search(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            List<MyContacts> results = await Contacts.GetContacts(new AuthorizationParameters(PromptBehavior.Auto, this.Handle));


            SearchResults.ItemsSource = results;
        }

        public IntPtr Handle
        {
            get
            {
                var interopHelper = new WindowInteropHelper(this);
                return interopHelper.Handle;
            }
        }
    }
}
