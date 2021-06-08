using Hotel.Models;
using Hotel.ViewModels;
using Hotel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    [Route("/offers")]
    public class OffersController : Controller
    {
        private readonly HttpClient _httpClient;

        public OffersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _httpClient.DefaultRequestHeaders.Add(
                ServerApiConfig.TokenHeaderName,
                HttpContext.User.Claims.First(c => c.Type == HotelCookieTokenManagerOptions.AuthStringClaimType).Value);
        }


        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] bool? isActive, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            return await this.TrySendAsync(async () =>
            {
                IEnumerable<OfferPreview> response = await _httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>($"offers?{query}");
                OffersIndexViewModel offersVM = new OffersIndexViewModel(response, paging, isActive);
                return View(offersVM);
            });
        }

        [HttpGet("{offerID}")]
        public async Task<IActionResult> Details([FromRoute] int offerID)
        {
            return await this.TrySendAsync(async () =>
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>($"offers/{offerID}");
                offer.OfferID = offerID;
                return View(offer);
            });
        }

        [HttpGet("{offerID}/edit")]
        public async Task<IActionResult> Edit([FromRoute] int offerID)
        {
            return await this.TrySendAsync(async () =>
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>($"offers/{offerID}");
                offer.OfferID = offerID;
                IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>($"offers/{offerID}/rooms");
                return View(new OfferEditViewModel(offer));
            });
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("{offerID}/edit")]
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

            return await this.TrySendAsync(async () =>
            {
                HttpResponseMessage response = await _httpClient.PatchAsync($"offers/{offerViewModel.Offer.OfferID}", content);
                return StatusCode((int)response.StatusCode);
            });
        }

        [HttpPost("{offerID}/changeActive")]
        public async Task<IActionResult> ChangeActive([FromRoute] int offerID, [FromQuery] bool isActive)
        {
            OfferUpdateInfo offer = new OfferUpdateInfo
            {
                IsActive = isActive
            };
            HttpContent content = JsonContent.Create(offer);

            await this.TrySendAsync(async () =>
            {
                await _httpClient.PatchAsync($"offers/{offerID}", content);
                return StatusCode((int)Response.StatusCode);
            });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] Offer offer)
        {
            return await this.TrySendAsync(async () =>
            {
                await _httpClient.PostAsJsonAsync("offers", offer);
                return RedirectToAction(nameof(Index));
            });
        }

        [HttpGet("{offerID}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int offerID)
        {
            return await this.TrySendAsync(async () =>
            {
                await _httpClient.DeleteAsync($"offers/{offerID}");
                return RedirectToAction(nameof(Index));
            });
        }

        [HttpGet("{offerID}/rooms")]
        public async Task<IActionResult> GetOfferRooms([FromRoute] int offerID, [FromQuery] Paging paging)
        {
            return await this.TrySendAsync(async () =>
            {
                IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>(
                    $"offers/{offerID}/rooms?pageNumber={paging.PageNumber}&pageSize={paging.PageSize}");
                return new JsonResult(rooms);
            });
        }

        [HttpDelete("{offerID}/rooms/{roomID}")]
        public async Task<IActionResult> DetachRoom([FromRoute] int offerID, [FromRoute] int roomID)
        {
            return await this.TrySendAsync(async () =>
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"offers/{offerID}/rooms/{roomID}");
                return StatusCode((int)response.StatusCode);
            });
        }

        [HttpPost("{offerID}/rooms")]
        public async Task<IActionResult> AttachRoom([FromRoute]int offerID, [FromForm]string roomNumber)
        {
            return await this.TrySendAsync(async () =>
            {
                Room[] room = await _httpClient.GetFromJsonAsync<Room[]>($"rooms?roomNumber={roomNumber}");
                if (room is null || !room.Any())
                    return StatusCode((int)HttpStatusCode.NotFound);
                int roomID = room[0].RoomID;
                return await this.TrySendAsync(async () =>
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"offers/{offerID}/rooms", roomID);
                    return StatusCode((int)response.StatusCode);
                });
            });
        }
    }
}
