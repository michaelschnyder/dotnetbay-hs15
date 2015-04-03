using System;
using System.Data.Entity;
using System.Linq;

using DotNetBay.Interfaces;
using DotNetBay.Model;

namespace DotNetBay.Data.EF
{
    public class EFMainRepository : IMainRepository
    {
        private readonly MainDbContext context;

        public EFMainRepository()
        {
            this.context = new MainDbContext();
        }

        public Database Database
        {
            get
            {
                return this.context.Database;
            }
        }

        public IQueryable<Auction> GetAuctions()
        {
            return this.context.Auctions.Include(a => a.Bids).Include(a => a.Seller).Include(a => a.ActiveBid).Include(a => a.ActiveBid.Bidder).Include(a => a.Winner);
        }

        public IQueryable<Member> GetMembers()
        {
            return this.context.Members.Include(m => m.Auctions).Include(m => m.Bids);
        }

        public Bid GetBidByTransactionId(Guid transactionId)
        {
            return this.context.Bids.Include(b => b.Bidder).Include(b => b.Auction).FirstOrDefault(b => b.TransactionId == transactionId);
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public Member Add(Member member)
        {
            this.context.Members.Add(member);

            return member;
        }

        public Bid Add(Bid bid)
        {
            this.context.Bids.Add(bid);

            return bid;
        }

        public Auction Update(Auction auction)
        {
            return auction;
        }

        public Auction Add(Auction auction)
        {
            this.context.Auctions.Add(auction);

            return auction;
        }
    }
}
