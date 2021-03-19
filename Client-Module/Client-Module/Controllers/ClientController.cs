using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Controllers
{
    public class ClientController : Controller
    {
        private ILogger<ClientController> _logger;
        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return Ok("Login GET");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            return Ok("Logout GET");
        }

        [HttpPost]
        public IActionResult LogIn(int nouse)
        {
            return Ok("Login POST");
        }

        [HttpGet]
        [Route("/account")]
        public IActionResult Account()
        {
            return Ok("client info page");
        }
    }
}
