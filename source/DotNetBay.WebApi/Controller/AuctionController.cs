using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        private readonly IMemberService memberService;

        public AuctionController()
        {
            var repo = new EFMainRepository();
            this.memberService = new SimpleMemberService(repo);

            this.auctionService = new AuctionService(repo, this.memberService);
        }

        [HttpGet]
        [Route("api/auctions")]
        public IHttpActionResult GetAllAuctions()
        {
            var allAuctions = this.auctionService.GetAll().ToList();

            var auctionsDto = new List<AuctionDto>();

            foreach (var auction in allAuctions)
            {
                auctionsDto.Add(this.MapAuctionToDto(auction));
            }

            return this.Ok(auctionsDto);
        }

        [HttpPost]
        [Route("api/auctions")]
        public IHttpActionResult AddNewAuction([FromBody] AuctionDto dto)
        {
            var theNewAuction = new Auction
            {
                Seller = this.memberService.GetCurrentMember(), 
                EndDateTimeUtc = dto.EndDateTimeUtc, 
                StartDateTimeUtc = dto.StartDateTimeUtc, 
                Title = dto.Title, 
                StartPrice = dto.StartPrice
            };

            try
            {
                this.auctionService.Save(theNewAuction);
                return this.Created(string.Format("api/auctions/{0}", theNewAuction.Id), this.MapAuctionToDto(theNewAuction));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/auctions/{id}")]
        public IHttpActionResult Auction(long id)
        {
            var auction = this.auctionService.GetAll().FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                return this.Ok(this.MapAuctionToDto(auction));
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
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(auction.Image);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("api/auctions/{id}/image")]
        public async Task<IHttpActionResult> AddImageForAuction(long id)
        {
            var auction = this.auctionService.GetAll().FirstOrDefault(a => a.Id == id);

            if (auction != null)
            {
                var streamProvider = await this.Request.Content.ReadAsMultipartAsync(); // HERE
                foreach (var file in streamProvider.Contents)
                {
                    var image = await file.ReadAsByteArrayAsync();
                    auction.Image = image;

                    this.auctionService.Save(auction);
                }

                return this.Ok();
            }

            return this.NotFound();
        }

        private AuctionDto MapAuctionToDto(Auction auction)
        {
            var dto = new AuctionDto
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
                CurrentWinnerName = auction.ActiveBid != null ? auction.ActiveBid.Bidder.DisplayName : null, 
                IsClosed = auction.IsClosed, 
                IsRunning = auction.IsRunning, 
                Bids = new List<BidDto>(), 
            };


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
