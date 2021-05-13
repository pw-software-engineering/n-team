using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.Services.Result;
using Server.ViewModels;

namespace Server.Controllers.Client
{
    [ApiController]
    [Route("/api-client")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private int _clientID;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = from claim in HttpContext.User.Claims
                     where claim.Type == ClientTokenManagerOptions.ClientIdClaimName
                     select int.Parse(claim.Value);
            _clientID = id.First();
            base.OnActionExecuting(context);
        }

        [HttpPost("hotels/{hotelID:int}/offers/{offerID:int}")]
        public IActionResult AddReservation([FromRoute] int hotelID, [FromRoute] int offerID, [FromQuery] ReservationInfo reservation)
        {
            return _reservationService.AddReservation(hotelID, offerID, _clientID, reservation);
        }

        [HttpDelete("reservations/{reservationID:int}")]
        public IActionResult CancelReservation([FromRoute] int reservationID)
        {
            return _reservationService.CancelReservation(reservationID, _clientID);
        }
    }
}
