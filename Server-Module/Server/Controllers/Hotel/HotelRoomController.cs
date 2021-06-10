using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.Services.Hotel;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-hotel")]
    public class HotelRoomController : Controller
    {
        private readonly IRoomService _service;
        private int _hotelID;
        public HotelRoomController(IRoomService service)
        {
            _service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hotelID = int.Parse(HttpContext.User.Claims.First(c => c.Type == HotelTokenManagerOptions.HotelIdClaimName).Value);
            base.OnActionExecuting(context);
        }

        [HttpGet("rooms")]
        public IActionResult GetHotelRooms([FromQuery] string roomNumber, [FromQuery] Paging paging)
        {
            return _service.GetHotelRooms(_hotelID, paging, roomNumber);
        }

        [HttpPost("rooms")]
        public IActionResult AddHotelRooms([FromBody] HotelRoom hotelRoom)
        {
            return _service.AddRoom(_hotelID, hotelRoom.HotelRoomNumber);
        }

        [HttpDelete("rooms/{roomID}")]
        public IActionResult DeleteHotelRoom([FromRoute] int roomID)
        {
            return _service.DeleteRoom(_hotelID, roomID);
        }

    }
}
