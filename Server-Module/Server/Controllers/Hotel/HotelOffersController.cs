using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.OfferService;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService service;

        public HotelOffersController(IOfferService service)
        {
            this.service = service;
        }

        [HttpGet("api-hotel/offers")]
        public IActionResult GetOffers(bool? isActive, int pageNumber = 1, int pageSize = 10)
        {
            int hotelId = GetHotelID();

            return service.GetHotelOffers(new Paging(pageSize, pageNumber), hotelId, isActive);
        }
        [HttpGet("api-hotel/offers/{offerID}")]
        public IActionResult GetOffer(int offerID)
        {
            int hotelId = GetHotelID();

            return service.GetOffer(offerID, hotelId);
        }

        [HttpPost("api-hotel/offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            int hotelId = GetHotelID();

            return service.AddOffer(offer, hotelId);
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
