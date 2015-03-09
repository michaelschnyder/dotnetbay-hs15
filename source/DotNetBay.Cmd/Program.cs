using System;
using System.Linq;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.EF;
using DotNetBay.Data.FileStorage;

namespace DotNetBay.Cmd
{
    /// <summary>
    /// Main Entry for program
    /// </summary>
    public class Program
    {

        public static void Main(string[] args)
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            Console.WriteLine("DotNetBay Commandline");

            var store = new EFMainRepository("Dnead.AuctionDb-Local");
            
            var auctionService = new AuctionService(store, new SimpleMemberService(store));
            var auctionRunner = new AuctionRunner(store);
            
            Console.WriteLine("Started AuctionRunner");
            auctionRunner.Start();

            var allAuctions = auctionService.GetAll();

            Console.WriteLine("Found {0} auctions returned by the service.", allAuctions.Count());

            Console.Write("Press enter to quit");
            Console.ReadLine();

            Environment.Exit(0);
        }
    }
}
