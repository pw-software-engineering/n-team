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
        public IActionResult Patch([FromBody] string username, [FromBody] string email)
        {
            // default id until authentication is implemented
            var id = 4;

            IServiceResult serviceResult = service.UpdateClientInfo(id, username, email);
            IActionResult result = BadRequest();
            if (serviceResult.StatusCode == HttpStatusCode.OK) result = Ok();

            return result;
        }
    }
}
