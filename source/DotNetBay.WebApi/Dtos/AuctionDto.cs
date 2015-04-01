using System;
using System.Collections.Generic;

namespace DotNetBay.WebApi.Dtos
{
    public class AuctionDto
    {
        public long Id { get; set; }

        public double StartPrice { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double CurrentPrice { get; set; }

        /// <summary>
        /// Gets or sets the UTC DateTime values to avoid wrong data when serializing the values
        /// </summary>
        public DateTime StartDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC DateTime values to avoid wrong data when serializing the values
        /// </summary>
        public DateTime EndDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC DateTime values to avoid wrong data when serializing the values
        /// </summary>
        public DateTime CloseDateTimeUtc { get; set; }

        public string SellerName { get; set; }

        public string FinalWinnerName { get; set; }

        public string CurrentWinnerName { get; set; }

        public List<BidDto> Bids { get; set; }

        public BidDto ActiveBid { get; set; }

        public bool IsClosed { get; set; }

        public bool IsRunning { get; set; }
    }
}