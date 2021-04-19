using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private HttpClient httpClient;

        public LoginController(ILogger<LoginController> logger, HttpClient httpClient)
        {
            _logger = logger;
            this.httpClient = httpClient;
            //wysłanie zapytania
            var t = httpClient.GetAsync("endpoint");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        { 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
