﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication.Client;
using Server.Services.ClientService;
using Server.Services.Result;

namespace Server.Controllers.Client
{
    [ApiController]
    [Route("api-client")]
    public class ClientLoginController : Controller
    {
        private readonly IClientService _clientService;
        public ClientLoginController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("client/login")]
        public IActionResult Login([FromBody] ClientCredentials credentials)
        {
            return _clientService.Login(credentials.Username, credentials.Password);
        }

        public class ClientCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
