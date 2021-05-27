using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory clientFactory;
        private HttpClient httpClient;
        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var result = HttpContext.User.Claims.Where(x => x.Type == "authString").ToArray();
            httpClient = clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("x-hotel-token", result[0].Value);

        }
        [HttpGet("/profile")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
