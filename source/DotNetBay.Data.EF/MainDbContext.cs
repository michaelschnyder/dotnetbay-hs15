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

            modelBuilder.Entity<Auction>().HasMany(a => a.Bids).WithRequired(b => b.Auction);
            modelBuilder.Entity<Auction>().HasRequired(a => a.Seller).WithMany(member => member.Auctions);

            modelBuilder.Entity<Member>().HasKey(m => m.Id);
            modelBuilder.Entity<Member>().HasKey(m => m.UniqueId);
        }
    }
}
