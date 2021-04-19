using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class ReservationsController : Controller
    {

        private HttpClient httpClient;
        public IActionResult Index()
        {
            return View();
        }
    }
}
