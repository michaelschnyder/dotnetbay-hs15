using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using DotNetBay.Data.EF;
using DotNetBay.Core.Execution;
using System.Data.Entity.SqlServer;

namespace DotNetBay.AzureWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("DotNetBay.AzureWorker is running");

            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = SqlProviderServices.Instance;

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("DotNetBay.AzureWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("DotNetBay.AzureWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("DotNetBay.AzureWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // DotNetBay startup
            var mainRepository = new EFMainRepository();
            mainRepository.SaveChanges();

            var auctionRunner = new AuctionRunner(mainRepository);

            ////auctionRunner.Auctioneer.AuctionStarted += (sender, args) => AuctionsHub.NotifyAuctionStarted(args.Auction);
            ////auctionRunner.Auctioneer.AuctionEnded += (sender, args) => AuctionsHub.NotifyAuctionEnded(args.Auction);
            ////auctionRunner.Auctioneer.BidAccepted += (sender, args) => AuctionsHub.NotifyNewBid(args.Auction, args.Bid);

            auctionRunner.Auctioneer.AuctionStarted += (sender, args) => Trace.TraceInformation("Auction has started");
            auctionRunner.Auctioneer.AuctionEnded += (sender, args) => Trace.TraceInformation("Auction has endded");
            auctionRunner.Auctioneer.BidAccepted += (sender, args) => Trace.TraceInformation("Bid has been accepted");

            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                auctionRunner.RunOnce();
                await Task.Delay(3000);
            }
        }
    }
}
