using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services.ClientService;
using Server.Services.Result;
using Server.Authentication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        private readonly IClientService service;

        public ClientAccountController(IClientService service)
        {
            this.service = service;
        }
        
        [HttpGet("/client")]
        public string Get()
        {
            return "";
        }
        
        [HttpPatch("/client")]
        public IActionResult Patch(string username, string email)
        {
            var id = from claim in HttpContext.User.Claims
                      where claim.Type == "id"
                      select int.Parse(claim.Value);

            return service.UpdateClientInfo(id.First(), username, email);
        }
    }
}
