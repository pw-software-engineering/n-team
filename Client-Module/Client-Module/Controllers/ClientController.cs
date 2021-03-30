using Client_Module.ViewsTagID.Layout;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("/login")]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpGet("/logout")]
        public IActionResult LogOut()
        {
            this.Response.Cookies.Delete(ClientTokenCookieDefaults.AuthCookieName);
            return Redirect("/");
        }

        [HttpPost("/login")]
        public IActionResult LogIn(string username, string password)
        {
            Console.WriteLine($"{username} | {password}");

            CookieOptions options = new CookieOptions();
            Response.Cookies.Append(ClientTokenCookieDefaults.AuthCookieName, "value2", options);
            return Redirect("/");
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
