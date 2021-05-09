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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Controllers.Client
{
    [Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class ClientAccountController : Controller
    {
        private readonly IClientService service;
        private int _clientID;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = from claim in HttpContext.User.Claims
                     where claim.Type == ClientTokenManagerOptions.ClientIdClaimName
                     select int.Parse(claim.Value);
            _clientID = id.First();
            base.OnActionExecuting(context);
        }
        public ClientAccountController(IClientService service)
        {
            this.service = service;
            
        }
        
        [HttpGet("client")]
        public IActionResult GetClientInfo()
        {
            return service.GetClientInfo(_clientID);
        }
        
        [HttpPatch("client")]
        public IActionResult PatchClientInfo(string username, string email)
        {
            return service.UpdateClientInfo(_clientID, username, email);
        }
    }
}
