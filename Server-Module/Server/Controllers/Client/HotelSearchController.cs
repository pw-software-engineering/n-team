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
    public class HotelSearchController : Controller
    {
        private readonly IHotelSearchService _service;
        public HotelSearchController(IHotelSearchService hotelSearchService)
        {
            _service = hotelSearchService;
        }

        [HttpGet("hotels")]
        public IActionResult GetHotels([FromQuery] HotelFilter hotelFilter, [FromQuery] Paging paging)
        {
            return _service.GetHotels(hotelFilter, paging);
        }

        [HttpGet("hotels/{hotelID:int}")]
        public IActionResult GetHotelDetails([FromRoute] int hotelID)
        {
            return _service.GetHotelDetails(hotelID);
        }
        [HttpGet("hotels/{hotelID:int}/reviews")]
        public IActionResult GetHotelReviews([FromRoute] int hotelID, [FromQuery] Paging paging)
        {
            return _service.GetHotelReviews(hotelID, paging);
        }
    }
}
