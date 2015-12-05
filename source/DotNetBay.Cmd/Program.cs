// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.EF;
using DotNetBay.Data.EF.Migrations;

namespace DotNetBay.Cmd
{
    /// <summary>
    /// Main Entry for program
    /// </summary>
    public static class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AuctionRunner", Justification = "Reviewed. This is fine here.")]
        public static void Main()
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = SqlProviderServices.Instance;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainDbContext, Configuration>());
            
            Console.WriteLine("DotNetBay Commandline");

            AuctionRunner auctionRunner = null;

            try
            {
                var store = new EFMainRepository();
                var auctionService = new AuctionService(store, new SimpleMemberService(store));

                auctionRunner = new AuctionRunner(store);

                Console.WriteLine("Started AuctionRunner");
                auctionRunner.Start();

                var allAuctions = auctionService.GetAll();

                Console.WriteLine("Found {0} auctions returned by the service.", allAuctions.Count());

                Console.Write("Press enter to quit");
                Console.ReadLine();
            }
            finally
            {
                if (auctionRunner != null)
                {
                    auctionRunner.Dispose();
                }
            }

            Environment.Exit(0);
        }
    }
}
