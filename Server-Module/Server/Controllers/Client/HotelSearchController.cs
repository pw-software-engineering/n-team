using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.Services.Result;
using Server.ViewModels;

namespace Server.Controllers.Client
{
    //[Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class HotelSearchController : Controller
    {
        private readonly IHotelSearchService _hotelSearchService;
        public HotelSearchController(IHotelSearchService hotelSearchService)
        {
            _hotelSearchService = hotelSearchService;
        }

        [HttpGet("hotels")]
        public IActionResult GetHotels([FromQuery] HotelFilter hotelFilter, [FromQuery] Paging paging)
        {
            return _hotelSearchService.GetHotels(hotelFilter, paging);
        }

        [HttpGet("hotels/{hotelID:int}")]
        public IActionResult GetHotelDetails([FromRoute] int hotelID)
        {
            return _hotelSearchService.GetHotelDetails(hotelID);
        }
    }
}
