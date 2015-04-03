using System.Linq;

using DotNetBay.Core;
using DotNetBay.Model;

namespace DotNetBay.WPF.Services
{
    public class RemoteAuctionService : IAuctionService
    {
        public IQueryable<Auction> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Auction Save(Auction auction)
        {
            throw new System.NotImplementedException();
        }

        public Bid PlaceBid(Auction auction, double amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
