using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class OffersController : Controller
    {
        private readonly HttpClient httpClient;

        public OffersController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<OfferPreview> response = await httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>("offers");
            return View(response);
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
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/offers", offer);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
            return RedirectToAction("index");
        }
    }
}
