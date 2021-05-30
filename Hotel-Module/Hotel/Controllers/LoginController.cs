using Hotel.Models;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> logger;
        private IHttpClientFactory httpClientFactory;

        public LoginController(ILogger<LoginController> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            //przykłąd wysłanie zapytania
            //httpClient = httpClientFactory.CreateClient();
            //var t = httpClient.GetAsync("endpoint");
        }

        [HttpGet("/")]
        public IActionResult Home()
        {
            return Redirect("/profile");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] string authString)
        {
            if(authString is null)
            {
                return View("~/Views/Login/Index.cshtml");
            }

            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
                httpClient.DefaultRequestHeaders.Add(ServerApiConfig.TokenHeaderName, authString);
                HttpResponseMessage httpResponse = await httpClient.GetAsync("rooms?pageNumber=1&pageSize=1");
                if (httpResponse.IsSuccessStatusCode)
                {
                    CookieOptions options = new CookieOptions();
                    Response.Cookies.Append(
                        HotelTokenCookieDefaults.AuthCookieName,
                        authString,
                        options);
                    return Redirect("/profile");
                }
                return View("~/Views/Login/Index.cshtml", new LogInError() { Error = httpResponse.StatusCode.ToString() });
            }
            catch
            {
                return View("~/Views/Login/Index.cshtml", new LogInError() { Error = "Server is unreachable" });
            }
        }

        [HttpGet("/logout")]
        public IActionResult LogOut()
        {
            this.Response.Cookies.Delete(HotelTokenCookieDefaults.AuthCookieName);
            return Redirect("/");
        }
    }

    public class LogInError
    {
        public string Error { get; set; }
    }
}
