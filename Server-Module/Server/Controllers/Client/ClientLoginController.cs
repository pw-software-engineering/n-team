using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.Services.Result;

namespace Server.Controllers.Client
{
    [ApiController]
    [Route("api-client")]
    public class ClientLoginController : Controller
    {
        private readonly IClientAccountService _clientService;
        public ClientLoginController(IClientAccountService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("client/login")]
        public IActionResult Login([FromBody] ClientCredentials credentials)
        {
            return _clientService.Login(credentials);
        }
    }
}
