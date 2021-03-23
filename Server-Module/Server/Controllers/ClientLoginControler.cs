using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class ClientLoginControler : ControllerBase
    {
        
        private readonly ILogger<ClientLoginControler> _logger;

        public ClientLoginControler(ILogger<ClientLoginControler> logger)
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
