using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using DotNetBay.Core;
using DotNetBay.Data.EF;
using DotNetBay.WebApi.Dtos;

namespace DotNetBay.WebApi.Controller
{
    public class AuctionController : ApiController
    {
        private readonly AuctionService service;

        public AuctionController()
        {
            var repo = new EFMainRepository();
            var members = new SimpleMemberService(repo);

            this.service = new AuctionService(repo, members);
        }

        [HttpGet]
        [Route("api/auctions")]
        public IHttpActionResult GetAllAuctions()
        {
            var allAuctions = this.service.GetAll().ToList();

            var auctionsDto = new List<AuctionDto>();

            foreach (var auction in allAuctions)
            {
                var dto = new AuctionDto()
                              {
                                  Id = auction.Id,
                                  StartPrice = auction.StartPrice,
                                  Title = auction.Title,
                                  Description = auction.Description,
                                  CurrentPrice = auction.Id,
                                  StartDateTimeUtc = auction.StartDateTimeUtc,
                                  EndDateTimeUtc = auction.EndDateTimeUtc,
                                  CloseDateTimeUtc = auction.CloseDateTimeUtc,
                                  SellerName = auction.Seller.DisplayName,
                                  WinnerName = auction.Winner.DisplayName,
                                  IsClosed = auction.IsClosed,
                                  IsRunning = auction.IsRunning,
                              };

                dto.Bids = new List<BidDto>();

                foreach (var bid in auction.Bids)
                {
                    dto.Bids.Add(new BidDto() { Id = bid.Id, TransactionId = bid.TransactionId, ReceivedOnUtc = bid.ReceivedOnUtc, BidderName = bid.Bidder.DisplayName, Accepted = bid.Accepted, Amount = bid.Amount});
                }

                auctionsDto.Add(dto);
            }

            return this.Ok(auctionsDto);
        }
    }
}
