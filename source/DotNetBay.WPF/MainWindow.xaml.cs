using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Model;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Auction> auctions;

        private AuctionService auctionService;

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = this;

            var app = Application.Current as App;

            app.AuctionRunner.Auctioneer.AuctionEnded += this.AuctioneerOnAuctionClosed;
            app.AuctionRunner.Auctioneer.AuctionStarted += this.AuctioneerOnAuctionStarted;
            app.AuctionRunner.Auctioneer.BidAccepted += this.AuctioneerOnBidAccepted;
            app.AuctionRunner.Auctioneer.BidDeclined += this.AuctioneerOnBidDeclined;

            this.auctionService = new AuctionService(app.MainRepository, new SimpleMemberService(app.MainRepository));

            this.auctions = new ObservableCollection<Auction>(this.auctionService.GetAll());
        }

        private void AuctioneerOnBidDeclined(object sender, ProcessedBidEventArgs processedBidEventArgs)
        {
            var allAuctionsFromService = this.auctionService.GetAll();
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);
        }

        private void AuctioneerOnBidAccepted(object sender, ProcessedBidEventArgs processedBidEventArgs)
        {
            var allAuctionsFromService = this.auctionService.GetAll();
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);
        }

        private void AuctioneerOnAuctionStarted(object sender, AuctionEventArgs auctionEventArgs)
        {
            var allAuctionsFromService = this.auctionService.GetAll();
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);
        }

        private void AuctioneerOnAuctionClosed(object sender, AuctionEventArgs auctionEventArgs)
        {
            var allAuctionsFromService = this.auctionService.GetAll();
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);
        }

        public ObservableCollection<Auction> Auctions
        {
            get
            {
                return this.auctions;
            }

            private set
            {
                this.auctions = value;
                this.OnPropertyChanged();
            }
        }

        private void AddNewAuctionButtonClick(object sender, RoutedEventArgs e)
        {
            var sellView = new SellView();
            sellView.ShowDialog(); // Blocking

            var allAuctionsFromService = this.auctionService.GetAll();

            /* Option A: Full Update via INotifyPropertyChanged, not performant */
            /* ================================================================ */
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);

            /////* Option B: Let WPF only update the List and detect the additions */
            /////* =============================================================== */
            ////var toAdd = allAuctionsFromService.Where(a => !this.auctions.Contains(a));
            ////foreach (var auction in toAdd)
            ////{
            ////    this.auctions.Add(auction);
            ////}
        }

        private void PlaceBidButtonClick(object sender, RoutedEventArgs e)
        {
            var currentAuction = (Auction)this.AuctionsDataGrid.SelectedItem;

            var sellView = new BidView(currentAuction);
            sellView.ShowDialog(); // Blocking
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
