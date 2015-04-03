using System.Windows;

using DotNetBay.WPF.Services;
using DotNetBay.WPF.ViewModel;

namespace DotNetBay.WPF.Views
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class SellView : Window
    {
        public SellView()
        {
            this.InitializeComponent();

            this.DataContext = new SellViewModel(new RemoteAuctionService());
        }
    }
}
