using Hotel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class OffersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        //[DisableRequestSizeLimit]
        public IActionResult Add([FromForm] Offer offer)
        {
            //IFormFile file = new OfferAdd().PreviewPicture;
            //var ms = new MemoryStream();
            //Convert.ToBase64String()
            return null;
        }
    }
}
