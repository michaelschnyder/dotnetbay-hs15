namespace DotNetBay.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartPrice = c.Double(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Image = c.Binary(),
                        CurrentPrice = c.Double(nullable: false),
                        StartDateTimeUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDateTimeUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CloseDateTimeUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsClosed = c.Boolean(nullable: false),
                        IsRunning = c.Boolean(nullable: false),
                        ActiveBid_Id = c.Long(),
                        Seller_Id = c.Long(nullable: false),
                        Winner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.ActiveBid_Id)
                .ForeignKey("dbo.Members", t => t.Seller_Id, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.Winner_Id)
                .Index(t => t.ActiveBid_Id)
                .Index(t => t.Seller_Id)
                .Index(t => t.Winner_Id);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReceivedOnUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TransactionId = c.Guid(nullable: false),
                        Amount = c.Double(nullable: false),
                        Accepted = c.Boolean(),
                        Bidder_Id = c.Long(),
                        Auction_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Bidder_Id)
                .ForeignKey("dbo.Auctions", t => t.Auction_Id, cascadeDelete: true)
                .Index(t => t.Bidder_Id)
                .Index(t => t.Auction_Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UniqueId = c.String(),
                        DisplayName = c.String(),
                        EMail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "Winner_Id", "dbo.Members");
            DropForeignKey("dbo.Auctions", "Seller_Id", "dbo.Members");
            DropForeignKey("dbo.Bids", "Auction_Id", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "ActiveBid_Id", "dbo.Bids");
            DropForeignKey("dbo.Bids", "Bidder_Id", "dbo.Members");
            DropIndex("dbo.Bids", new[] { "Auction_Id" });
            DropIndex("dbo.Bids", new[] { "Bidder_Id" });
            DropIndex("dbo.Auctions", new[] { "Winner_Id" });
            DropIndex("dbo.Auctions", new[] { "Seller_Id" });
            DropIndex("dbo.Auctions", new[] { "ActiveBid_Id" });
            DropTable("dbo.Members");
            DropTable("dbo.Bids");
            DropTable("dbo.Auctions");
        }
    }
}
