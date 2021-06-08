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
    public class ReservationsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReservationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _httpClient.DefaultRequestHeaders.Add(
                ServerApiConfig.TokenHeaderName,
                HttpContext.User.Claims.First(c => c.Type == HotelCookieTokenManagerOptions.AuthStringClaimType).Value);
        }

        [Route("/reservations")]
        public async Task<IActionResult> Index(bool currentOnly, string roomNumber, Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (currentOnly)
                query["currentOnly"] = currentOnly.ToString();
            if (string.IsNullOrWhiteSpace(roomNumber))
                query["roomNumber"] = roomNumber;
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            IEnumerable<ReservationObject> reservations;
            try
            {
                reservations = await _httpClient.GetFromJsonAsync<IEnumerable<ReservationObject>>($"reservations?{query}");
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            ReservationIndexViewModel reservationIndexVM = new ReservationIndexViewModel
            {
                Reservations = reservations,
                Paging = paging,
                CurrentOnly = currentOnly,
                RoomNumber = roomNumber
            };

            return View(reservationIndexVM);
        }
    }
}
