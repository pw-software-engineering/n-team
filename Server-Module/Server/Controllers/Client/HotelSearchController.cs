using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.Services.HotelSearchService;
using Server.Services.Response;
using Server.ViewModels;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
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
        public IActionResult GetHotels([FromQuery] Paging paging, [FromQuery] HotelFilter hotelFilter)
        {
            //Console.WriteLine($"{paging.pageNumber} | {paging.pageSize} [|] {hotelFilter.City} | {hotelFilter.Country} | {hotelFilter.HotelName}");
            IServiceResult serviceResult = _hotelSearchService.GetHotels(paging, hotelFilter);
            JsonResult jsonResult = new JsonResult(serviceResult.ResponseBody)
            {
                StatusCode = (int)serviceResult.StatusCode
            };
            return jsonResult;
        }

        [HttpGet("hotels/{hotelID:int}")]
        public IActionResult GetHotelDetails(int hotelID)
        {
            IServiceResult serviceResult = _hotelSearchService.GetHotelDetails(hotelID);
            JsonResult jsonResult = new JsonResult(serviceResult.ResponseBody)
            {
                StatusCode = (int)serviceResult.StatusCode
            };
            return jsonResult;
        }
    }
}
