using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace Hotel.Controllers
{
    public class OffersController : Controller
    {
        private readonly HttpClient httpClient;

        public OffersController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }

        public async Task<IActionResult> Index(bool? isActive, int pageNumber = 1, int pageSize = 10)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = pageNumber.ToString();
            query["pageSize"] = pageSize.ToString();

            IEnumerable<OfferPreview> response = await httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>("offers?" + query.ToString());

            OffersIndex offersVM = new OffersIndex(response, new Paging(pageNumber, pageSize), isActive);
            return View(offersVM);
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
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("offers", offer);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
            return RedirectToAction("index");
        }
    }
}
