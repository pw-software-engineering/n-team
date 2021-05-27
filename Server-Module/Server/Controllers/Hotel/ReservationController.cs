using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Hotel;
using Server.RequestModels;
using Server.Services.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Route("/api-hotel")]
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
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
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == HotelTokenManagerOptions.HotelIdClaimName
                      select claim.Value;
            _hotelID = Convert.ToInt32(ids.Single());
            base.OnActionExecuting(context);
        }
        [HttpGet("reservations")]
        public IActionResult GetReservations([FromQuery] bool? currentOnly, [FromQuery] int? roomID, [FromQuery] Paging paging)
        {
            return _service.GetReservations(_hotelID, currentOnly, roomID, paging);
        }
    }
}
