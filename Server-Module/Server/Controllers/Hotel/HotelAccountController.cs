using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Authentication;
using Microsoft.Extensions.Logging;

namespace Server.Controllers.Hotel
{
    [Authorize(AuthenticationSchemes = "HotellTokenScheme")]
    //[Route("api/[controller]")]
    [ApiController]
    public class HotelAccountController : ControllerBase
    {
        private readonly ILogger<HotelAccountController> _logger;
        private IHttpContextAccessor httpContextAccessor1;

        public HotelAccountController(ILogger<HotelAccountController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor1 = httpContextAccessor;
            _logger = logger;
        }

        [Route("/hotelInfo")]
        [HttpPatch()]
        public IActionResult UpdateInfo([FromBody] int tu_dane_z_body)
        {
            var user = httpContextAccessor1.HttpContext.User.Identity;
            return StatusCode(404);
        }
    }
}
