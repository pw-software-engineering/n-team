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
    public class ReservationController : Controller
    {
        private readonly IReservationService _service;
        private int _hotelID;
        public ReservationController(IReservationService service)
        {
            _service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hotelID = int.Parse(HttpContext.User.Claims.First(c => c.Type == HotelTokenManagerOptions.HotelIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        [HttpGet("reservations")]
        public IActionResult GetReservations([FromQuery] bool? currentOnly, [FromQuery] int? roomID, [FromQuery] Paging paging)
        {
            return _service.GetReservations(_hotelID, currentOnly, roomID, paging);
        }
    }
}
