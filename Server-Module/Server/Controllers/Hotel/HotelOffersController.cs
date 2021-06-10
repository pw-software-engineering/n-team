using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.Services.Hotel;
using System;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-hotel")]
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
            _hotelID = int.Parse(HttpContext.User.Claims.First(c => c.Type == HotelTokenManagerOptions.HotelIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        [HttpGet("offers")]
        public IActionResult GetOffers([FromQuery] bool? isActive, [FromQuery] Paging paging)
        {
            return _service.GetHotelOffers(_hotelID, paging, isActive);
        }
        [HttpGet("offers/{offerID}")]
        public IActionResult GetOffer([FromRoute] int offerID)
        {
            return _service.GetOffer(_hotelID, offerID);
        }

        [HttpPost("offers")]
        public IActionResult AddOffer([FromBody] OfferInfo offer)
        {
            return _service.AddOffer(_hotelID, offer);
        }

        [HttpPatch("offers/{offerID}")]
        public IActionResult EditOffer([FromRoute] int offerID, [FromBody] OfferInfoUpdate offerInfoUpdate)
        {
            return _service.UpdateOffer(_hotelID, offerID, offerInfoUpdate);
        }

        [HttpDelete("offers/{offerID}")]
        public IActionResult DeleteOffer([FromRoute] int offerID)
        {
            return _service.DeleteOffer(_hotelID, offerID);
        }
    }
}
