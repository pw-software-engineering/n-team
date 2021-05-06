using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.RequestModels;
using Server.Services.OfferService;
using Server.ViewModels;
using System;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Route("/api-hotel/")]
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService service;

        public HotelOffersController(IOfferService service)
        {
            this.service = service;
        }

        [HttpGet("offers")]
        public IActionResult GetOffers(bool? isActive, int pageNumber = 1, int pageSize = 10)
        {
            int hotelId = GetHotelID();

            return service.GetHotelOffers(new Paging(pageSize, pageNumber), hotelId, isActive);
        }
        [HttpGet("offers/{offerID}")]
        public IActionResult GetOffer(int offerID)
        {
            int hotelId = GetHotelID();
            return service.GetOffer(offerID, hotelId);
        }

        [HttpPost("offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            int hotelId = GetHotelID();

            return service.AddOffer(offer, hotelId);
        }

        [HttpPatch("offers/{offerID}")]
        public IActionResult EditOffer(int offerID, OfferUpdateInfo updateInfo)
        {
            int hotelId = GetHotelID();

            return service.UpdateOffer(offerID, hotelId, updateInfo);
        }

        [HttpDelete("offers/{offerID}")]
        public IActionResult DeleteOffer(int offerID)
        {
            int hotelId = GetHotelID();

            return service.DeleteOffer(offerID, hotelId);
        }

        private int GetHotelID()
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "hotelId"
                      select claim.Value;
            return Convert.ToInt32(ids.Single());
        }
    }
}
