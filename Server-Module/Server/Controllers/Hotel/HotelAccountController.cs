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
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    public class HotelAccountController : ControllerBase
    {
        private readonly ICustomAuthenticationManager customAuthenticationManager;
        private readonly ILogger<HotelAccountController> _logger;

        public HotelAccountController(ILogger<HotelAccountController> logger, ICustomAuthenticationManager customAuthenticationManager)
        {
            this.customAuthenticationManager = customAuthenticationManager;
            _logger = logger;
        }

        [Route("/hotelInfo")]
        [HttpPatch()]
        public IActionResult UpdateInfo([FromBody] int tu_dane_z_body)
        {
            return StatusCode(404);
        }
    }
}
