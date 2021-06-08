using Hotel.Models;
using Hotel.ViewModels;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            try
            {
                HotelInfo hotelInfo = await _httpClient.GetFromJsonAsync<HotelInfo>("hotelInfo");
                return View(hotelInfo);
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

        [HttpGet("/profile/edit")]
        public async Task<IActionResult> Edit()
        {
            try
            {
                HotelInfo hotelInfo = await _httpClient.GetFromJsonAsync<HotelInfo>("hotelInfo");
                HotelEditViewModel hotelEdit = new HotelEditViewModel(hotelInfo);
                return View(hotelEdit);
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

        [HttpPost("/profile/edit")]
        public async Task<IActionResult> Edit([FromForm] HotelEditViewModel hotelEdit)
        {
            HotelInfo hotelInfo = hotelEdit.HotelInfo;
            hotelInfo.HotelPreviewPicture = hotelEdit.ChangePreviewPicture ? (hotelEdit.HotelInfo.HotelPreviewPicture ?? "") : null;
            hotelInfo.HotelPictures = hotelEdit.ChangeHotelPictures ? (hotelEdit.HotelInfo.HotelPictures ?? new List<string>()) : null;
            
            JsonContent content = JsonContent.Create(hotelInfo);
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PatchAsync("hotelInfo", content);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return StatusCode((int)response.StatusCode);
        }
    }
}
