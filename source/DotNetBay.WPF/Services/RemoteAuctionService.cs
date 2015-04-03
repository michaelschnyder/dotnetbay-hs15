using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;

using DotNetBay.Core;
using DotNetBay.Model;

using Newtonsoft.Json;

namespace DotNetBay.WPF.Services
{
    public class RemoteAuctionService : IAuctionService
    {
        private readonly HttpClient httpClient;

        public RemoteAuctionService()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri("http://localhost:64168/");
        }

        public IQueryable<Auction> GetAll()
        {
            var response = this.httpClient.GetAsync("api/auctions").Result;
            response.EnsureSuccessStatusCode();

            string content = response.Content.ReadAsStringAsync().Result;

            var auctionDtoList = JsonConvert.DeserializeObject<List<AuctionDto>>(content);

            return auctionDtoList.AsParallel().Select(this.MapFromDto).ToList().AsQueryable();
        }

        public Auction Save(Auction auction)
        {
            var url = "api/auctions/";

            var dto = new AuctionDto()
                {
                    StartPrice = auction.StartPrice,
                    StartDateTimeUtc = auction.StartDateTimeUtc,
                    EndDateTimeUtc = auction.EndDateTimeUtc,
                    Title = auction.Title,
                    Description = auction.Description,
                };

            var result = this.httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json")).Result;

            if (result.IsSuccessStatusCode)
            {
                var rawJson = result.Content.ReadAsStringAsync().Result;
                var responseDto = JsonConvert.DeserializeObject<AuctionDto>(rawJson);

                // Post Image
                if (auction.Image != null)
                {
                    this.SendAuctionImage(responseDto.Id, auction.Image);
                }

                return this.MapFromDto(responseDto);
            }

            throw new Exception(result.Content.ReadAsStringAsync().Result);
        }

        public Bid PlaceBid(Auction auction, double amount)
        {
            var url = string.Format("api/auctions/{0}/bids", auction.Id);
            var dto = new BidDto() { Amount = amount };

            var result = this.httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json")).Result;

            if (result.IsSuccessStatusCode)
            {
                var rawJson = result.Content.ReadAsStringAsync().Result;
                var responseDto = JsonConvert.DeserializeObject<BidDto>(rawJson);

                return this.MapFromDto(responseDto);
            }

            throw new Exception(result.Content.ReadAsStringAsync().Result);
        }

        private Auction MapFromDto(AuctionDto dto)
        {
            return new Auction()
                {
                    Id = dto.Id,
                    Title = dto.Title, 
                    CurrentPrice = dto.CurrentPrice,
                    CloseDateTimeUtc = dto.CloseDateTimeUtc,
                    StartDateTimeUtc = dto.StartDateTimeUtc,
                    StartPrice = dto.StartPrice,
                    IsClosed = dto.IsClosed,
                    IsRunning = dto.IsRunning,
                    Description = dto.Description,
                    EndDateTimeUtc = dto.EndDateTimeUtc,
                    ActiveBid = new Bid() { Bidder = new Member() { DisplayName = dto.CurrentWinnerName } },
                    Winner = new Member() { DisplayName = dto.CurrentWinnerName },
                    Seller = new Member() { DisplayName = dto.SellerName },
                    Image = this.GetAuctionImage(dto.Id),
                    Bids = this.MapFromDto(dto.Bids),
                };
        }

        private Bid MapFromDto(BidDto bid)
        {
            return new Bid()
                {
                    Accepted = bid.Accepted,
                    Amount = bid.Amount,
                    ReceivedOnUtc = bid.ReceivedOnUtc,
                    Id = bid.Id,
                    Bidder = new Member() { DisplayName = bid.BidderName }
                };
        }

        private List<Bid> MapFromDto(List<BidDto> bids)
        {
            if (bids != null)
            {
                return bids.Select(this.MapFromDto).ToList();
            }

            return null;
        }

        private byte[] GetAuctionImage(long id)
        {
            try
            {
                return new HttpClient() { BaseAddress = this.httpClient.BaseAddress }.GetByteArrayAsync(string.Format("api/auctions/{0}/image", id)).Result;
            }
            catch
            {
                return null;
            }
        }

        private void SendAuctionImage(long auctionId, byte[] imageContents)
        {
            using (var client = new HttpClient { BaseAddress = this.httpClient.BaseAddress })
            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new ByteArrayContent(imageContents), "image", "image.jpg");

                var url = string.Format("api/auctions/{0}/image", auctionId);
                var result = client.PostAsync(url, multipartContent).Result;

                result.EnsureSuccessStatusCode();
            }
        }
    }
}
