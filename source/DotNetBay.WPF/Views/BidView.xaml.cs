using System.Windows;

using DotNetBay.Core;
using DotNetBay.Model;
using DotNetBay.WPF.Services;
using DotNetBay.WPF.ViewModel;

namespace DotNetBay.WPF.Views
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class BidView : Window
    {
        public BidView(Auction selectedAuction)
        {
            this.InitializeComponent();

            var auctionService = new RemoteAuctionService();

            this.DataContext = new BidViewModel(selectedAuction, auctionService);
        }
    }
}
