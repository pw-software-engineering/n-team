using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Hotel.Controllers
{
    public class OffersController : Controller
    {
        private readonly HttpClient httpClient;

        public OffersController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

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
        public IActionResult Add([FromForm] Offer offer)
        {
            //httpClient.PostAsync("/offers")
            //IFormFile file = new OfferAdd().PreviewPicture;
            //var ms = new MemoryStream();
            //Convert.ToBase64String()
            return null;
        }
    }
}
