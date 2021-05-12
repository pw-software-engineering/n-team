using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
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
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService _service;
        private int _hotelID;
        public HotelOffersController(IOfferService service)
        {
            _service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == HotelTokenManagerOptions.HotelIdClaimName
                      select claim.Value;
            _hotelID = Convert.ToInt32(ids.Single());
            base.OnActionExecuting(context);
        }
        [HttpGet("offers")]
        public IActionResult GetOffers([FromQuery] bool? isActive, [FromQuery] Paging paging)
        {
            return _service.GetHotelOffers(paging, _hotelID, isActive);
        }
        [HttpGet("offers/{offerID}")]
        public IActionResult GetOffer([FromRoute] int offerID)
        {
            return _service.GetOffer(offerID, _hotelID);
        }

        [HttpPost("offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            return _service.AddOffer(offer, _hotelID);
        }

        [HttpPatch("offers/{offerID}")]
        public IActionResult EditOffer([FromRoute] int offerID, [FromBody] OfferUpdateInfo updateInfo)
        {
            return _service.UpdateOffer(offerID, _hotelID, updateInfo);
        }

        [HttpDelete("offers/{offerID}")]
        public IActionResult DeleteOffer([FromRoute] int offerID)
        {
            return _service.DeleteOffer(offerID, _hotelID);
        }
    }
}
