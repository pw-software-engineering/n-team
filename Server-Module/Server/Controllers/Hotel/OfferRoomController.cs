using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
using Server.RequestModels;
using Server.Services.Hotel;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-hotel")]
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
            _hotelID = int.Parse(HttpContext.User.Claims.First(c => c.Type == HotelTokenManagerOptions.HotelIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        [HttpGet("offers/{offerID:int}/rooms")]
        public IActionResult GetOfferRooms([FromRoute] int offerID, [FromQuery] string hotelRoomNumber, [FromQuery] Paging paging)
        {
            return _service.GetOfferRooms(offerID, _hotelID, hotelRoomNumber, paging);
        }
        [HttpPost("offers/{offerID:int}/rooms")]
        public IActionResult AddRoomToOffer([FromRoute] int offerID, [FromBody] int roomID)
        {
            return _service.AddRoomToOffer(roomID, offerID, _hotelID);
        }
        [HttpDelete("offers/{offerID:int}/rooms/{roomID:int}")]
        public IActionResult RemoveRoomFromOffer([FromRoute] int offerID, [FromRoute] int roomID)
        {
            return _service.RemoveRoomFromOffer(roomID, offerID, _hotelID);
        }
    }
}
