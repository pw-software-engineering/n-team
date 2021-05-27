using Hotel.Models;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult OfferRowPartial(uint offerID)
        {
            // get data from server
            Offer offer = new Offer
            {
                OfferPreviewPicture = "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAARElEQVR42u3PMREAAAgEIE1u9DeDqwcN6FSmHmgRERERERERERERERERERERERERERERERERERERERERERERERERkYsFbE58nZm0+8AAAAAASUVORK5CYII=",
                OfferTitle = "Limitless Offer",
                CostPerAdult = 120.00,
                CostPerChild = 100.00,
                MaxGuests = 3
            };

            return PartialView("_OfferRow", offer);
        }
    }
}
