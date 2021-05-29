using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class ClientReviewControler : ControllerBase
    {
        private int _clientID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = from claim in HttpContext.User.Claims
                     where claim.Type == ClientTokenManagerOptions.ClientIdClaimName
                     select int.Parse(claim.Value);
            _clientID = id.First();
            base.OnActionExecuting(context);
        }
        [HttpGet("reservations/{reservationID}/review")]
        public IActionResult GetReview()
        {
            return null;
        }

        [HttpPut("reservations/{reservationID}/review")]
        public IActionResult CreateReview()
        {
            return null;
        }

        [HttpDelete("reservations/{reservationID}/review")]
        public IActionResult DeleteReview()
        {
            return null;
        }
    }
}
