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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    //[Route("api/[controller]")]
    [ApiController]
    public class HotelAccountController : Controller
    {
        private readonly ILogger<HotelAccountController> _logger;
        private IHotelAccountService _hotelAccountService;
        private int _hotelID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hotelID = int.Parse(HttpContext.User.Claims.First().Value);
            base.OnActionExecuting(context);
        }
        public HotelAccountController(ILogger<HotelAccountController> logger, IHotelAccountService hotelAccountService)
        {
            _hotelAccountService = hotelAccountService;
            _logger = logger;
        }

        [HttpPatch("/hotelInfo")]
        public IActionResult UpdateInfo([FromBody] HotelUpdateInfo hotelUpdateInfo)
        {
            return _hotelAccountService.UpdateInfo(_hotelID, hotelUpdateInfo);
        }

        [HttpGet("/hotelInfo")]
        public IActionResult GetInfo()
        {
            return _hotelAccountService.GetInfo(_hotelID);
        }
    }
}
