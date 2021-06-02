using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Services.Hotel;
using Server.Authentication.Hotel;
using Server.RequestModels.Hotel;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = HotelTokenDefaults.AuthenticationScheme)]
    [Route("/api-hotel")]
    [ApiController]
    public class HotelAccountController : Controller
    {
        private IHotelAccountService _hotelAccountService;
        private int _hotelID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hotelID = int.Parse(HttpContext.User.Claims.First().Value);
            base.OnActionExecuting(context);
        }
        public HotelAccountController(IHotelAccountService hotelAccountService)
        {
            _hotelAccountService = hotelAccountService;
        }

        [HttpPatch("hotelInfo")]
        public IActionResult UpdateHotelInfo([FromBody] HotelInfoUpdate hotelInfoUpdate)
        {
            return _hotelAccountService.UpdateHotelInfo(_hotelID, hotelInfoUpdate);
        }

        [HttpGet("hotelInfo")]
        public IActionResult GetHotelInfo()
        {
            return _hotelAccountService.GetHotelInfo(_hotelID);
        }
    }
}
