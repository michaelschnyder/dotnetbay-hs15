using System;

namespace DotNetBay.WebApi.Dtos
{
    public class BidDto
    {
        public long Id { get; set; }

        public DateTime ReceivedOnUtc { get; set; }

        public Guid TransactionId { get; set; }

        public string BidderName { get; set; }

        public double Amount { get; set; }

        public bool? Accepted { get; set; }

        public string AuctionTitle { get; set; }
    }
}