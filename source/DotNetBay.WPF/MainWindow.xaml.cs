using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Model;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Auction> auctions = new ObservableCollection<Auction>();

        private AuctionService auctionService;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            var app = Application.Current as App;

            app.AuctionRunner.Auctioneer.AuctionClosed += AuctioneerOnAuctionClosed;
            app.AuctionRunner.Auctioneer.BidAccepted += AuctioneerOnBidAccepted;
            app.AuctionRunner.Auctioneer.BidDeclined += AuctioneerOnBidDeclined;

            this.auctionService = new AuctionService(app.MainRepository, new SimpleMemberService(app.MainRepository));

            this.auctions = new ObservableCollection<Auction>(this.auctionService.GetAll());
        }

        private void AuctioneerOnBidDeclined(object sender, ProcessedBidEventArgs processedBidEventArgs)
        {

        }

        private void AuctioneerOnBidAccepted(object sender, ProcessedBidEventArgs processedBidEventArgs)
        {
        }

        private void AuctioneerOnAuctionClosed(object sender, AuctionEventArgs auctionEventArgs)
        {
            
        }

        public ObservableCollection<Auction> Auctions
        {
            get
            {
                return this.auctions;
            }
        }

        private void AddNewAuctionButton_Click(object sender, RoutedEventArgs e)
        {
            var sellView = new SellView();
            sellView.ShowDialog(); // Blocking

            this.auctions = new ObservableCollection<Auction>(this.auctionService.GetAll());

        }

        private void PlaceBidButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
