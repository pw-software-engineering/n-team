using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class OfferSearchController : Controller
    {
        private readonly IOfferSearchService _service;
        public OfferSearchController(IOfferSearchService offerSearchService)
        {
            _service = offerSearchService;
        }
        
        [HttpGet("hotels/{hotelID:int}/offers")]
        public IActionResult GetHotelOffers([FromRoute] int hotelID, [FromQuery] OfferFilter offerFilter, [FromQuery] Paging paging)
        {
            return _service.GetHotelOffers(hotelID, offerFilter, paging);
        }

        [HttpGet("hotels/{hotelID:int}/offers/{offerID:int}")]
        public IActionResult GetHotelOfferDetails([FromRoute] int hotelID, [FromRoute] int offerID)
        {
            return _service.GetHotelOfferDetails(hotelID, offerID);
        }

        [HttpGet("hotel/{hotelID:int}/offers/{offerID:int}/reviews")]
        public IActionResult GetHotelOfferReviews([FromRoute] int hotelID, [FromRoute] int offerID, [FromQuery] Paging paging)
        {
            return _service.GetHotelOfferReviews(hotelID, offerID, paging);
        }
    }
}
