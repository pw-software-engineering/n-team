using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Client_Module.Controllers
{
    public class SearchController : Controller
    {
        [Route("/hotels")]
        public IActionResult Hotels()
        {
            return View();
        }

        [Route("/hotels/{hotelID}/offers")]
        public IActionResult HotelOffers(int hotelID)
        {
            return View();
        }

        [Route("/hotels/{hotelID}/offers/{offerID}")]
        public IActionResult DetailedHotelOffer(int hotelID, int offerID)
        {
            return View();
        }
    }
}
