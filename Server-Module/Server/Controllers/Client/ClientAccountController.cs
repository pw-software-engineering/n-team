using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services.ClientService;
using Server.Services.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [ApiController]
    //[Route("[controller]")]
    public class ClientAccountController : ControllerBase
    {
        private readonly IClientService service;
        private readonly ILogger<ClientAccountController> _logger;

        public ClientAccountController(IClientService service, ILogger<ClientAccountController> logger)
        {
            this.service = service;
            _logger = logger;
        }
		[Route("/client")]
		[HttpGet]
		public string Get()
		{
			return "";
		}
        [Route("/client")]
        [HttpPatch]
        public IActionResult Patch(string username, string email)
        {
            // default id until authentication is implemented
            var id = 2;

            IServiceResult serviceResult = service.UpdateClientInfo(id, username, email);

            return StatusCode((int)serviceResult.StatusCode, serviceResult.ResponseBody);
        }
    }
}
