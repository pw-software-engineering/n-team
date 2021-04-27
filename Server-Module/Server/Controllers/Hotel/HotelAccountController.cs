using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Authentication;
using Microsoft.Extensions.Logging;
using Server.ViewModels;
using Server.Services.HotelAccountService;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    //[Route("api/[controller]")]
    [ApiController]
    public class HotelAccountController : ControllerBase
    {
        private readonly ILogger<HotelAccountController> _logger;
        private IHotelAccountService hotelAccountService;
        public HotelAccountController(ILogger<HotelAccountController> logger, IHotelAccountService hotelAccountService)
        {
            this.hotelAccountService = hotelAccountService;
            _logger = logger;
        }

        [HttpPatch("/hotelInfo")]
        public IActionResult UpdateInfo([FromBody] HotelUpdateInfo hotelUpdateInfo)
        {
            var hotelId = int.Parse(HttpContext.User.Claims.First().Value);
            try
            {
                hotelAccountService.UpdateInfo(hotelId, hotelUpdateInfo);
            }catch(Exception)
            {
                return StatusCode(404);
            }
            
            return StatusCode(200);
        }

        [HttpGet("/hotelInfo")]
        public IActionResult GetInfo()
        {
            var hotelId = int.Parse(HttpContext.User.Claims.First().Value);
            try
            {
                return new JsonResult(hotelAccountService.GetInfo(hotelId));
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }
    }
}
