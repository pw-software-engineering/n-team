using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Client;
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
    [Route("/api-client")]
    public class ClientReviewControler : Controller
    {
        #region setup
        private IReviewSerice _reviewService;
        public ClientReviewControler(IReviewSerice reviewService)
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
        public IActionResult GetReview([FromHeader]int reservationID)
        {
            return _reviewService.GetReview(reservationID);
        }

        [HttpPut("reservations/{reservationID}/review")]
        public IActionResult CreateReview([FromHeader] int reservationID,[FromBody]ReviewUpdater reviewUpdater)
        {
            return _reviewService.PutReview(reservationID, _clientID,reviewUpdater);
        }

        [HttpDelete("reservations/{reservationID}/review")]
        public IActionResult DeleteReview([FromHeader] int reservationID)
        {
            return _reviewService.DeleteReview(reservationID, _clientID);
        }
        #endregion
    }
}
