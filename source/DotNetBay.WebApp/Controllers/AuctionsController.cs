using System;
using System.Linq;
using System.Web.Mvc;

using DotNetBay.Core;
using DotNetBay.Data.EF;
using DotNetBay.Model;

namespace DotNetBay.WebApp.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly IAuctionService service;

        private readonly EFMainRepository mainRepository;

        public AuctionsController()
        {
            this.mainRepository = new EFMainRepository();

            this.service = new AuctionService(this.mainRepository, new SimpleMemberService(this.mainRepository));
        }

        // GET: Auctions
        public ActionResult Index()
        {
            return View(this.service.GetAll().ToList());
        }

        // GET: Auctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartPrice,Title,Description,StartDateTimeUtc,EndDateTimeUtc")] Auction auction)
        {
            var members = new SimpleMemberService(this.mainRepository);
            auction.Seller = members.GetCurrentMember();

            this.service.Save(auction);

            return RedirectToAction("Index");
        }
    }
}
