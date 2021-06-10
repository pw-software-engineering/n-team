using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authentication.Client;
using Server.RequestModels.Client;
using Server.Services.Client.ClientReviewService;
using System.Linq;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client/client")]
    public class ClientReviewControler : Controller
    {
        #region setup
        private IReviewService _service;
        private int _clientID;
        public ClientReviewControler(IReviewService reviewService)
        {
            _service = reviewService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _clientID = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClientTokenManagerOptions.ClientIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        #endregion
        #region reservations/{reservationID}/review
        [HttpGet("reservations/{reservationID:int}/review")]
        public IActionResult GetReview([FromRoute] int reservationID)
        {
            return _service.GetReview(reservationID, _clientID);
        }

        [HttpPut("reservations/{reservationID:int}/review")]
        public IActionResult CreateReview([FromRoute] int reservationID, [FromBody] ReviewUpdate reviewUpdate)
        {
            return _service.PutReview(reservationID, _clientID, reviewUpdate);
        }

        [HttpDelete("reservations/{reservationID:int}/review")]
        public IActionResult DeleteReview([FromRoute] int reservationID)
        {
            return _service.DeleteReview(reservationID, _clientID);
        }
        #endregion
    }
}
