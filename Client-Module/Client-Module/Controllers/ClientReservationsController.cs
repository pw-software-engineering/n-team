using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Client_Module.Controllers
{
    public class ClientReservationsController : Controller
    {
        [Route("/reservations")]
        public IActionResult Reservations()
        {
            return View();
        }
    }
}
