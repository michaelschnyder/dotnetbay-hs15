using System.Data.Entity;

using DotNetBay.Model;

namespace DotNetBay.Data.EF
{
    public class MainDbContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Add(new DateTime2Convention()); 
        }
    }
}
