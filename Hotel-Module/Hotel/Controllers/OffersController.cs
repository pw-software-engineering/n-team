using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
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

        [HttpGet("/offers")]
        public async Task<IActionResult> Index(bool? isActive, Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            IEnumerable<OfferPreview> response;
            try
            {
                response = await httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>("offers?" + query.ToString());
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode is null)
                    return StatusCode((int)HttpStatusCode.BadGateway);
                return StatusCode((int)e.StatusCode);
            }
            OffersIndex offersVM = new OffersIndex(response, paging, isActive);
            return View(offersVM);
        }

        [HttpGet("/offers/{offerID}")]
        public async Task<IActionResult> Details(int offerID)
        {
            try
            {
                Offer offer = await httpClient.GetFromJsonAsync<Offer>("offers/" + offerID.ToString());
                return View(offer);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode is null)
                    return StatusCode((int)HttpStatusCode.BadGateway);
                return StatusCode((int)e.StatusCode);
            }
        }

        [HttpGet("/offers/{offerID}/edit")]
        public async Task<IActionResult> Edit(int offerID)
        {
            try
            {
                Offer offer = await httpClient.GetFromJsonAsync<Offer>("offers/" + offerID.ToString());
                return View(new OfferEditViewModel(offer));
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode is null)
                    return StatusCode((int)HttpStatusCode.BadGateway);
                return StatusCode((int)e.StatusCode);
            }
        }

        [HttpPost("/offers/{offerID}/edit")]
        public async Task<IActionResult> Edit([FromForm] OfferEditViewModel offerViewModel)
        {
            OfferUpdateInfo offer = new OfferUpdateInfo
            {
                IsActive = offerViewModel.Offer.IsActive,
                OfferTitle = offerViewModel.Offer.OfferTitle,
                Description = offerViewModel.Offer.Description,
                OfferPreviewPicture = offerViewModel.ChangePreviewPicture ? (offerViewModel.Offer.OfferPreviewPicture ?? "") : null,
                OfferPictures = offerViewModel.ChangeOfferPictures ? (offerViewModel.Offer.Pictures ?? new List<string>()) : null
            };
            HttpContent content = JsonContent.Create(offer);

            try
            {
                HttpResponseMessage response = await httpClient.PatchAsync("offers/" + offerViewModel.Offer.OfferID.ToString(), content);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode);
                return RedirectToAction("Details", new { offerViewModel.Offer.OfferID });
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode is null)
                    return StatusCode((int)HttpStatusCode.BadGateway);
                return StatusCode((int)e.StatusCode);
            }
        }

        [HttpGet("/offers/add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("/offers/add")]
        public async Task<IActionResult> Add([FromForm] Offer offer)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync("offers", offer);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("index");
                return StatusCode((int)response.StatusCode);
            }
            catch (HttpRequestException)
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        [HttpGet("/offers/{offerID}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int offerID)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync("offers/" + offerID.ToString());
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("index");
                return StatusCode((int)response.StatusCode);
            }
            catch (HttpRequestException)
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }
    }
}
