using Hotel.Models;
using Hotel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    [Route("/rooms")]
    public class RoomsController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomsController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Index([FromQuery] string hotelRoomNumber, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (!string.IsNullOrWhiteSpace(hotelRoomNumber))
                query["roomNumber"] = hotelRoomNumber.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            return await this.TrySendAsync(async () =>
            {
                IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>($"rooms?{query}");
                RoomsIndexViewModel roomsVM = new RoomsIndexViewModel(rooms, paging, hotelRoomNumber);
                return View(roomsVM);
            });
        }

        [HttpDelete("{roomID}")]
        public async Task<IActionResult> RemoveRoom([FromRoute] int roomID)
        {

            return await this.TrySendAsync(async () =>
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"rooms/{roomID}");
                return StatusCode((int)response.StatusCode);
            });
        }

        [HttpPost("")]
        public async Task<IActionResult> AddRoom([FromForm] string roomNumber)
        {
            return await this.TrySendAsync(async () =>
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("rooms", new { hotelRoomNumber = roomNumber });
                return StatusCode((int)response.StatusCode);
            });
        }

        [HttpGet("offerRowPartial")]
        public async Task<PartialViewResult> OfferRowPartial(int offerID, int roomID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>($"offers/{offerID}");
                offer.OfferID = offerID;
                OfferRowViewModel offerRowVM = new OfferRowViewModel { Offer = offer, RoomID = roomID };
                ViewBag.ErrorCode = null;
                return PartialView("_OfferRow", offerRowVM);
            }
            catch (HttpRequestException e)
            {
                ViewBag.ErrorCode = (int)(e.StatusCode ?? HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                ViewBag.ErrorCode = (int)HttpStatusCode.InternalServerError;
            }
            return PartialView("_OfferRow", null);
        }
    }
}
