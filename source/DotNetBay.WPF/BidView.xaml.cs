using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

using DotNetBay.Core;
using DotNetBay.Model;

using Microsoft.Win32;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class BidView : Window
    {
        private readonly Auction selectedAuction;

        private readonly AuctionService auctionService;

        private SimpleMemberService simpleMemberService;

        public double YourBid { get; set; }

        public Auction SelectedAuction
        {
            get
            {
                return this.selectedAuction;
            }
        }

        public BidView(Auction selectedAuction)
        {
            this.selectedAuction = selectedAuction;
            this.InitializeComponent();

            this.DataContext = this;

            var app = Application.Current as App;

            this.simpleMemberService = new SimpleMemberService(app.MainRepository);
            this.auctionService = new AuctionService(app.MainRepository, simpleMemberService);

            this.YourBid = Math.Max(this.SelectedAuction.CurrentPrice, this.SelectedAuction.StartPrice);
        }

        private void PlaceBidAuction_Click(object sender, RoutedEventArgs e)
        {
            this.auctionService.PlaceBid(this.simpleMemberService.GetCurrentMember(), this.SelectedAuction, this.YourBid);

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
