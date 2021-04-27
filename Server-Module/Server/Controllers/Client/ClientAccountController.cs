using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Authentication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        
        private readonly ILogger<ClientAccountController> _logger;

        public ClientAccountController(ILogger<ClientAccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/client")]
        public string Get()
        {
            return "";
        }
    }
}
