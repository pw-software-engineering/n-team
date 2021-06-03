using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Client;
using Server.RequestModels.Client;
using Server.Services.Client.ClientReviewService;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client/client")]
    public class ClientReviewControler : Controller
    {
        #region setup
        private IReviewService _reviewService;
        public ClientReviewControler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private int _clientID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = from claim in HttpContext.User.Claims
                     where claim.Type == ClientTokenManagerOptions.ClientIdClaimName
                     select int.Parse(claim.Value);
            _clientID = id.First();
            base.OnActionExecuting(context);
        }
        #endregion
        #region reservations/{reservationID}/review
        [HttpGet("reservations/{reservationID}/review")]
        public IActionResult GetReview([FromRoute] int reservationID)
        {
            return _reviewService.GetReview(reservationID, _clientID);
        }

        [HttpPut("reservations/{reservationID}/review")]
        public IActionResult CreateReview([FromRoute] int reservationID, [FromBody] ReviewUpdate reviewUpdate)
        {
            return _reviewService.PutReview(reservationID, _clientID, reviewUpdate);
        }

        [HttpDelete("reservations/{reservationID}/review")]
        public IActionResult DeleteReview([FromRoute] int reservationID)
        {
            return _reviewService.DeleteReview(reservationID, _clientID);
        }
        #endregion
    }
}
