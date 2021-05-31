using Hotel.Models;
using Hotel.ViewModels;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
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

        [HttpGet("/offers")]
        public async Task<IActionResult> Index([FromQuery] bool? isActive, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            try
            {
                IEnumerable<OfferPreview> response = await _httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>($"offers?{query}");
                OffersIndexViewModel offersVM = new OffersIndexViewModel(response, paging, isActive);
                return View(offersVM);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("/offers/{offerID}")]
        public async Task<IActionResult> Details([FromRoute] int offerID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>($"offers/{offerID}");
                offer.OfferID = offerID;
                return View(offer);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("/offers/{offerID}/edit")]
        public async Task<IActionResult> Edit([FromRoute] int offerID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>($"offers/{offerID}");
                offer.OfferID = offerID;
                IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>($"offers/{offerID}/rooms");
                //IEnumerable<Room> rooms = new List<Room>
                //{
                //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
                //    new Room { HotelRoomNumber = "2", RoomID = 2, OfferID = new List<int> { offerID, 103 } }
                //};
                return View(new OfferEditViewModel(offer));
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
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

            return await CheckForConnectionError(_httpClient.PatchAsync($"offers/{offerViewModel.Offer.OfferID}", content));
        }

        [HttpPost("/offers/{offerID}/changeActive")]
        public async Task<IActionResult> ChangeActive([FromRoute] int offerID, [FromQuery] bool isActive)
        {
            OfferUpdateInfo offer = new OfferUpdateInfo
            {
                IsActive = isActive
            };
            HttpContent content = JsonContent.Create(offer);

            await CheckForConnectionError(_httpClient.PatchAsync($"offers/{offerID}", content));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/offers/add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("/offers/add")]
        public async Task<IActionResult> Add([FromForm] Offer offer)
        {
            return await CheckForConnectionError(_httpClient.PostAsJsonAsync("offers", offer));
        }

        [HttpGet("/offers/{offerID}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int offerID)
        {
            return await CheckForConnectionError(_httpClient.DeleteAsync($"offers/{offerID}"));
        }

        [HttpGet("/offers/{offerID}/rooms")]
        public async Task<IActionResult> GetOfferRooms([FromRoute] int offerID, [FromQuery] Paging paging)
        {
            IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>($"offers/{offerID}/rooms");
            //IEnumerable<Room> rooms = paging.PageNumber == 1 ? new List<Room>
            //{
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "2", RoomID = 2, OfferID = new List<int> { offerID, 103 } }
            //} : paging.PageNumber == 2 ?
            //new List<Room>
            //{
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "1", RoomID = 1, OfferID = new List<int> { offerID, 102 } },
            //    new Room { HotelRoomNumber = "2", RoomID = 2, OfferID = new List<int> { offerID, 103 } }
            //} :
            //new List<Room>();
            return new JsonResult(rooms);
        }

        [HttpDelete("/offers/{offerID}/rooms/{roomID}")]
        public async Task<IActionResult> DetachRoom([FromRoute] int offerID, [FromRoute] int roomID)
        {
            return await CheckForConnectionError(_httpClient.DeleteAsync($"offers/{offerID}/rooms/{roomID}"));
        }

        [HttpPost("/offers/{offerID}/rooms")]
        public async Task<IActionResult> AttachRoom([FromRoute]int offerID, [FromForm]string roomNumber)
        {
            Room[] room;
            try
            {
                room = await _httpClient.GetFromJsonAsync<Room[]>($"offers/{offerID}/rooms?roomNumber={roomNumber}");
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return await CheckForConnectionError(_httpClient.PostAsJsonAsync($"offers/{offerID}/rooms", new { roomID = room[0].RoomID }));
        }

        private async Task<StatusCodeResult> CheckForConnectionError(Task<HttpResponseMessage> responseTask)
        {
            try
            {
                HttpResponseMessage response = await responseTask;
                return StatusCode((int)response.StatusCode);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
