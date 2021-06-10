using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _service;
        private int _clientID;
        public ReservationController(IReservationService reservationService)
        {
            _service = reservationService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _clientID = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClientTokenManagerOptions.ClientIdClaimName).Value);
            base.OnActionExecuting(context);
        }

        [HttpPost("hotels/{hotelID:int}/offers/{offerID:int}/reservations")]
        public IActionResult AddReservation([FromRoute] int hotelID, [FromRoute] int offerID, [FromBody] ReservationInfo reservation)
        {
            return _service.AddReservation(hotelID, offerID, _clientID, reservation);
        }

        [HttpGet("client/reservations")]
        public IActionResult GetReservations([FromQuery] Paging paging)
        {
            return _service.GetReservations(_clientID, paging);
        }

        [HttpDelete("client/reservations/{reservationID:int}")]
        public IActionResult CancelReservation([FromRoute] int reservationID)
        {
            return _service.CancelReservation(reservationID, _clientID);
        }
    }
}
