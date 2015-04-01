using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

using DotNetBay.Core;
using DotNetBay.Data.EF;
using DotNetBay.Model;
using DotNetBay.WebApi.Dtos;

namespace DotNetBay.WebApi.Controller
{
    public class AuctionController : ApiController
    {
        private readonly IAuctionService auctionService;

        private readonly SimpleMemberService memberService;

        public AuctionController()
        {
            var repo = new EFMainRepository();
            this.memberService = new SimpleMemberService(repo);

            this.auctionService = new AuctionService(repo, memberService);
        }

        [HttpGet]
        [Route("api/auctions")]
        public IHttpActionResult GetAllAuctions()
        {
            var allAuctions = this.auctionService.GetAll().ToList();

            var auctionsDto = new List<AuctionDto>();

            foreach (var auction in allAuctions)
            {
                auctionsDto.Add(this.Map(auction));
            }

            return this.Ok(auctionsDto);
        }

        [HttpPost]
        [Route("api/auctions")]
        public IHttpActionResult AddNewAuction([FromBody] AuctionDto dto)
        {
            var theNewAuction = new Auction();

            theNewAuction.Seller = this.memberService.GetCurrentMember();
            theNewAuction.EndDateTimeUtc = dto.EndDateTimeUtc;
            theNewAuction.StartDateTimeUtc = dto.StartDateTimeUtc;
            theNewAuction.Title = dto.Title;
            theNewAuction.StartPrice = dto.StartPrice;

            this.auctionService.Save(theNewAuction);

            return this.Created(string.Format("api/auctions/{0}", theNewAuction.Id), this.Map(theNewAuction));
        }

        [HttpGet]
        [Route("api/auctions/{id}")]
        public IHttpActionResult Auction(long id)
        {
            var auction = this.auctionService.GetAll().FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                return this.Ok(this.Map(auction));
            }

            return this.NotFound();
        }

        [HttpGet]
        [Route("api/auctions/{id}/image")]
        public HttpResponseMessage ImageForAuction(long id)
        {
            var auction = this.auctionService.GetAll().FirstOrDefault(a => a.Id == id);

            if (auction != null && auction.Image != null)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(auction.Image);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("api/auctions/{id}/image")]
        public IHttpActionResult AddImageForAuction(long id)
        {
            var auction = this.auctionService.GetAll().FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                IEnumerable<HttpContent> parts = this.Request.Content.ReadAsMultipartAsync().Result.Contents;

                foreach (var part in parts)
                {
                    var result = part.ReadAsByteArrayAsync().Result;

                    auction.Image = result;

                    this.auctionService.Save(auction);

                    return this.Ok();
                }
            }

            return this.NotFound();
        }

        private AuctionDto Map(Auction auction)
        {
            var dto = new AuctionDto()
                          {
                              Id = auction.Id,
                              StartPrice = auction.StartPrice,
                              Title = auction.Title,
                              Description = auction.Description,
                              CurrentPrice = auction.CurrentPrice,
                              StartDateTimeUtc = auction.StartDateTimeUtc,
                              EndDateTimeUtc = auction.EndDateTimeUtc,
                              CloseDateTimeUtc = auction.CloseDateTimeUtc,
                              SellerName = auction.Seller != null ? auction.Seller.DisplayName : null,
                              FinalWinnerName = auction.Winner != null ? auction.Winner.DisplayName : null,
                              CurrentWinnerName =
                                  auction.ActiveBid != null ? auction.ActiveBid.Bidder.DisplayName : null,
                              IsClosed = auction.IsClosed,
                              IsRunning = auction.IsRunning,
                          };

            dto.Bids = new List<BidDto>();

            foreach (var bid in auction.Bids)
            {
                dto.Bids.Add(
                    new BidDto()
                        {
                            Id = bid.Id,
                            TransactionId = bid.TransactionId,
                            ReceivedOnUtc = bid.ReceivedOnUtc,
                            BidderName = bid.Bidder.DisplayName,
                            Accepted = bid.Accepted,
                            Amount = bid.Amount
                        });
            }

            return dto;
        }
    }
}
