using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
    public class RoomsController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }


        public async Task<IActionResult> Index([FromQuery] string roomNumber, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (!string.IsNullOrWhiteSpace(roomNumber))
                query["roomNumber"] = roomNumber.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            try
            {
                IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>("rooms?" + query.ToString());
                RoomsIndexViewModel roomsVM = new RoomsIndexViewModel(rooms, paging, roomNumber);
                return View(roomsVM);
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

        public PartialViewResult OfferRowPartial(uint offerID)
        {
            // get data from server
            Offer offer = new Offer
            {
                OfferPreviewPicture = "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAARElEQVR42u3PMREAAAgEIE1u9DeDqwcN6FSmHmgRERERERERERERERERERERERERERERERERERERERERERERERERkYsFbE58nZm0+8AAAAAASUVORK5CYII=",
                OfferTitle = "Limitless Offer",
                CostPerAdult = 120.00,
                CostPerChild = 100.00,
                MaxGuests = 3
            };

            return PartialView("_OfferRow", offer);
        }
    }
}
