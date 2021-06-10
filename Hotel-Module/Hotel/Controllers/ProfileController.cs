using Hotel.Models;
using Hotel.ViewModels;
using Hotel.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    [Route("/profile")]
    public class ProfileController : Controller
    {
        private HttpClient _httpClient;

        public ProfileController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Index()
        {
            return await this.TrySendAsync(async () =>
            {
                HotelInfo hotelInfo = await _httpClient.GetFromJsonAsync<HotelInfo>("hotelInfo");
                return View(hotelInfo);
            });
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            return await this.TrySendAsync(async () =>
            {
                HotelInfo hotelInfo = await _httpClient.GetFromJsonAsync<HotelInfo>("hotelInfo");
                HotelEditViewModel hotelEdit = new HotelEditViewModel(hotelInfo);
                return View(hotelEdit);
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromForm] HotelEditViewModel hotelEdit)
        {
            HotelInfo hotelInfo = hotelEdit.HotelInfo;
            hotelInfo.HotelPreviewPicture = hotelEdit.ChangePreviewPicture ? (hotelEdit.HotelInfo.HotelPreviewPicture ?? "") : null;
            hotelInfo.HotelPictures = hotelEdit.ChangeHotelPictures ? (hotelEdit.HotelInfo.HotelPictures ?? new List<string>()) : null;
            
            JsonContent content = JsonContent.Create(hotelInfo);
            return await this.TrySendAsync(async () =>
            {
                HttpResponseMessage response = await _httpClient.PatchAsync("hotelInfo", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
                return StatusCode((int)response.StatusCode);
            });
        }
    }
}
