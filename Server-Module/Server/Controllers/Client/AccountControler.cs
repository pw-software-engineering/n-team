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
    public class AccountControler : ControllerBase
    {
        
        private readonly ILogger<AccountControler> _logger;

        public AccountControler(ILogger<AccountControler> logger)
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
