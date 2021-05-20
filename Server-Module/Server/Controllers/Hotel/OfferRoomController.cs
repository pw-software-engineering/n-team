using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
using Server.RequestModels;
using Server.Services.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Route("/api-hotel")]
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    public class OfferRoomController : Controller
    {
        private readonly IOfferRoomService _service;
        private int _hotelID;
        public OfferRoomController(IOfferRoomService service)
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
        [HttpGet("offers/{offerID}/rooms")]
        public IActionResult GetOfferRooms([FromRoute] int offerID, [FromQuery] string hotelRoomNumber, [FromQuery] Paging paging)
        {
            return _service.GetOfferRooms(offerID, _hotelID, hotelRoomNumber, paging);
        }
        [HttpPost("offers/{offerID}/rooms")]
        public IActionResult AddRoomToOffer([FromRoute] int offerID, [FromBody] int roomID)
        {
            return _service.AddRoomToOffer(roomID, offerID, _hotelID);
        }
        [HttpDelete("offers/{offerID}/rooms/{roomID}")]
        public IActionResult RemoveRoomFromOffer([FromRoute] int offerID, [FromRoute] int roomID)
        {
            return _service.RemoveRoomFromOffer(roomID, offerID, _hotelID);
        }
    }
}
