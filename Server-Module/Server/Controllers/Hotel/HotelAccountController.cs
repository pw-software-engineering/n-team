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
    [ApiController]
    [Route("/api-hotel")]
    public class HotelAccountController : Controller
    {
        private IHotelAccountService _service;
        private int _hotelID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _hotelID = int.Parse(HttpContext.User.Claims.First(c => c.Type == HotelTokenManagerOptions.HotelIdClaimName).Value);
            base.OnActionExecuting(context);
        }
        public HotelAccountController(IHotelAccountService hotelAccountService)
        {
            _service = hotelAccountService;
        }

        [HttpGet("hotelInfo")]
        public IActionResult GetHotelInfo()
        {
            return _service.GetHotelInfo(_hotelID);
        }

        [HttpPatch("hotelInfo")]
        public IActionResult UpdateHotelInfo([FromBody] HotelInfoUpdate hotelInfoUpdate)
        {
            return _service.UpdateHotelInfo(_hotelID, hotelInfoUpdate);
        }
    }
}
