using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ClientInfoUpdateControler : ControllerBase
    {
        private readonly ILogger<ClientInfoUpdateControler> _logger;

        public ClientInfoUpdateControler(ILogger<ClientInfoUpdateControler> logger)
        {
            _logger = logger;
        }
        [Route("/client")]
        [HttpPost]
        public string Post()
        {
            return "";
        }
    }
}
