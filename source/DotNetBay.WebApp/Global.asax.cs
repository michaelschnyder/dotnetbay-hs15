using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.EF;
using DotNetBay.Interfaces;
using DotNetBay.SignalR.Hubs;

using Microsoft.AspNet.SignalR;

namespace DotNetBay.WebApp
{
    public class MvcApplication : HttpApplication
    {
        public static IAuctionRunner AuctionRunner { get; private set; }

        protected void Application_Start()
        {
            // MVC related startup
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // DotNetBay startup
            var mainRepository = new EFMainRepository();
            mainRepository.SaveChanges();

            AuctionRunner = new AuctionRunner(mainRepository);
            AuctionRunner.Start();

            AuctionRunner.Auctioneer.AuctionStarted += (sender, args) => AuctionsHub.NotifyAuctionStarted(args.Auction);
            AuctionRunner.Auctioneer.AuctionEnded += (sender, args) => AuctionsHub.NotifyAuctionEnded(args.Auction);

            AuctionRunner.Auctioneer.BidAccepted += (sender, args) => AuctionsHub.NotifyNewBid(args.Auction, args.Bid);
        }

        
    }
}
