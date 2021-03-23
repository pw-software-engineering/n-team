using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_Module.ViewsTagID.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Client_Module.Controllers
{
    public class ClientReservationsController : Controller
    {
        public ClientReservationsController()
        {
            //ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavReservationsBtnID;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavReservationsBtnID;
            base.OnActionExecuting(context);
        }

        [Route("/reservations")]
        public IActionResult Reservations()
        {
            
            //ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavReservationsBtnID;
            return View();
        }

        public IActionResult Reservation2()
        {
            return View();
        }
    }
}
