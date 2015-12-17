using DotNetBay.Model;

using Microsoft.AspNet.SignalR;

namespace DotNetBay.SignalR.Hubs
{
    public class AuctionsHub : Hub
    {
        public static void NotifyNewAuction(Auction auction)
        {
            GlobalHost.ConnectionManager.GetHubContext<AuctionsHub>()
                .Clients.All.NewAuction(auction.Id, auction.Title, auction.StartPrice);
        }

        public static void NotifyNewBid(Auction auction, Bid bid)
        {
            GlobalHost.ConnectionManager.GetHubContext<AuctionsHub>()
                .Clients.All.NewBid(auction.Id, auction.Title, bid.Amount, bid.Bidder.DisplayName);
        }

        public static void NotifyAuctionStarted(Auction auction)
        {
            GlobalHost.ConnectionManager.GetHubContext<AuctionsHub>()
                .Clients.All.AuctionStarted(auction.Id, auction.Title);
        }

        public static void NotifyAuctionEnded(Auction auction)
        {
            GlobalHost.ConnectionManager.GetHubContext<AuctionsHub>()
                .Clients.All.AuctionEnded(auction.Id, auction.Title, auction.ActiveBid.Amount, auction.ActiveBid.Bidder.DisplayName);
        }
    }
}