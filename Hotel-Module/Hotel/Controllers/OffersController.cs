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

        public async Task<IActionResult> Index(bool? isActive, Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            IEnumerable<OfferPreview> response = await httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>("offers?" + query.ToString());

            OffersIndex offersVM = new OffersIndex(response, paging, isActive);
            return View(offersVM);
        }

        [Route("offers/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Offer offer = await httpClient.GetFromJsonAsync<Offer>("offers/" + id.ToString());
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Add()
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
