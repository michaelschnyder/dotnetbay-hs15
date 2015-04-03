using System.Windows;

using DotNetBay.WPF.Services;
using DotNetBay.WPF.ViewModel;

namespace DotNetBay.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new MainViewModel(new RemoteAuctionService());
        }
    }
}
