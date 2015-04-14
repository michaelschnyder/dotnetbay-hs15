using DotNetBay.Model;

using Microsoft.AspNet.SignalR;

namespace DotNetBay.SignalR.Hubs
{
    public class AuctionsHub : Hub
    {
        public static void NotifyNewAuction(Auction auction)
        {
        }

        public static void NotifyNewBid(Auction auction, Bid bid)
        {
        }

        public static void NotifyAuctionStarted(Auction auction)
        {
        }

        public static void NotifyAuctionClosed(Auction auction)
        {
        }
    }
}