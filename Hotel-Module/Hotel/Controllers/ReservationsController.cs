using Hotel.Models;
using Hotel.ViewModels;
using Hotel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    [Route("/reservations")]
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


        [Route("")]
        public async Task<IActionResult> Index([FromQuery] bool currentOnly, [FromQuery] string roomNumber, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (currentOnly)
                query["currentOnly"] = currentOnly.ToString();
            if (string.IsNullOrWhiteSpace(roomNumber))
                query["roomNumber"] = roomNumber;
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            return await this.TrySendAsync(async () =>
            {
                IEnumerable<ReservationObject> reservations = await _httpClient.GetFromJsonAsync<IEnumerable<ReservationObject>>($"reservations?{query}");
                ReservationIndexViewModel reservationIndexVM = new ReservationIndexViewModel
                {
                    Reservations = reservations,
                    Paging = paging,
                    CurrentOnly = currentOnly,
                    RoomNumber = roomNumber
                };
                return View(reservationIndexVM);
            });

        }
    }
}
