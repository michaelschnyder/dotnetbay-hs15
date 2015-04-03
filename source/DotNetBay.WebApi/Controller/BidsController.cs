using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DotNetBay.Core;
using DotNetBay.Data.EF;
using DotNetBay.Interfaces;
using DotNetBay.Model;
using DotNetBay.WebApi.Dtos;

namespace DotNetBay.WebApi.Controller
{
    public class BidsController : ApiController
    {
        private readonly IAuctionService auctionService;

        private readonly IMemberService memberService;

        private IMainRepository repo;

        public BidsController()
        {
            this.repo = new EFMainRepository();
            this.memberService = new SimpleMemberService(repo);

            this.auctionService = new AuctionService(repo, memberService);
        }

        [HttpGet]
        [Route("api/auctions/{auctionId}/bids")]
        public IHttpActionResult GetAllBidsPerAuction(long auctionId)
        {
            var auction = this.repo.GetAuctions().FirstOrDefault(a => a.Id == auctionId);

            if (auction == null)
            {
                return this.NotFound();
            }

            var bids = auction.Bids;

            return this.Ok(bids.Select(MapBidToDto));
        }

        [HttpPost]
        [Route("api/auctions/{auctionId}/bids")]
        public IHttpActionResult PlaceBid(long auctionId, BidDto dto)
        {
            var auction = this.repo.GetAuctions().FirstOrDefault(a => a.Id == auctionId);

            if (auction == null)
            {
                return this.NotFound();
            }

            var currentMember = this.memberService.GetCurrentMember();

            try
            {
                var bid = this.auctionService.PlaceBid(currentMember, auction, dto.Amount);
                return this.Created(string.Format("api/bids/{0}", bid.TransactionId), MapBidToDto(bid));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/bids/{transactionId}")]
        public IHttpActionResult GetBidByTransationId(Guid transactionId)
        {
            var bid = this.repo.GetBidByTransactionId(transactionId);

            if (bid == null)
            {
                return this.NotFound();
            }

            return this.Ok(MapBidToDto(bid));
        }

        private static BidDto MapBidToDto(Bid bid)
        {
            return new BidDto()
            {
                Id = bid.Id, 
                Amount = bid.Amount, 
                Accepted = bid.Accepted, 
                BidderName = bid.Bidder.DisplayName, 
                ReceivedOnUtc = bid.ReceivedOnUtc, 
                TransactionId = bid.TransactionId, 
                AuctionTitle = bid.Auction.Title
            };
        }
    }
}
