using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.Services.HotelSearchService;
using Server.Services.ReservationService;
using Server.Services.Result;
using Server.ViewModels;

namespace Server.Controllers.Client
{
    [ApiController]
    [Route("/api-client")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("hotels/{hotelID:int}/offers/{offerID:int}")]
        public IActionResult AddReservation([FromRoute] int hotelID, [FromRoute] int offerID, [FromQuery] ReservationInfo reservation)
        {
            int userID = GetUserID();

            return _reservationService.AddReservation(hotelID, offerID, userID, reservation);
        }

        [HttpDelete("reservations/{reservationID:int}")]
        public IActionResult CancelReservation([FromRoute] int reservationID)
        {
            int userID = GetUserID();

            return _reservationService.CancelReservation(reservationID, userID);
        }
        private int GetUserID()
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "id"
                      select claim.Value;
            return Convert.ToInt32(ids.Single());
        }
    }
}
