using System.Data.Entity;
using System.Diagnostics;

using DotNetBay.Model;

namespace DotNetBay.Data.EF
{
    public class MainDbContext : DbContext
    {
        public MainDbContext() : this("Dnead.AuctionDb-Migrations")
        {
            // You shouldn't use this constructor in your code! This one is only to create and test migrations!
            // Debugger.Break();
        }

        public MainDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Auction> Auctions { get; set; }
        
        public DbSet<Member> Members { get; set; }
        
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new DateTime2Convention());

            modelBuilder.Entity<Auction>().HasRequired(a => a.Seller).WithMany(member => member.Auctions);
            modelBuilder.Entity<Auction>().HasMany(a => a.Bids).WithRequired(b => b.Auction);
        }
    }
}
