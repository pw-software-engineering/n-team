using Hotel.Models;
using Hotel.ViewModels;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net.Http;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
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

        [HttpGet("/profile")]
        public IActionResult Index()
        {
            HotelInfo hotelInfo = new HotelInfo();
            return View(hotelInfo);
        }

        [HttpGet("/profile/edit")]
        public IActionResult Edit()
        {
            HotelEditViewModel hotelEdit = new HotelEditViewModel
            {
                HotelInfo = new HotelInfo()
            };
            return View(hotelEdit);
        }

        [HttpPost("/profile/edit")]
        public IActionResult Edit([FromForm] HotelEditViewModel hotelEdit)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
