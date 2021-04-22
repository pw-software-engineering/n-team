using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Add([FromForm] Offer offer)
        {
            HttpContent content = JsonContent.Create(offer);
            
            HttpResponseMessage response = await httpClient.PostAsync("/offers", content);

            //IFormFile file = new OfferAdd().PreviewPicture;
            //var ms = new MemoryStream();
            //Convert.ToBase64String()
            return null;
        }
    }
}
