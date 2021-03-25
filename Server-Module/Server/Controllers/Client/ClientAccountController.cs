using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers.Client
{
    [ApiController]
    //[Route("[controller]")]
    public class ClientAccountController : ControllerBase
    {
        
        private readonly ILogger<ClientAccountController> _logger;

        public ClientAccountController(ILogger<ClientAccountController> logger)
        {
            _logger = logger;
        }
        [Route("/client")]
        [HttpGet]
        public string Get()
        {
            return "";
        }
    }
}
