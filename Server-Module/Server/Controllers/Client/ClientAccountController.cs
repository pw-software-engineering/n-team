using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services.ClientService;
using Server.Services.Response;
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
            // default id until authentication is implemented
            var id = 2;

            IServiceResult serviceResult = service.UpdateClientInfo(id, username, email);
            JsonResult jsonResult = new JsonResult(serviceResult.ResponseBody)
            {
                StatusCode = (int)serviceResult.StatusCode
            };

            return jsonResult;
        }
    }
}
