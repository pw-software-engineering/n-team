using Client_Module.ViewsTagID.Layout;
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

        [Route("/login")]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
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
            ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavAccountBtnID;
            return View();
        }
    }
}
