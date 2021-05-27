using Hotel.Models;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] string loginString)
        {
            ClientSecrets secrets = new ClientSecrets(loginString);
            HttpClient httpClient = httpClientFactory.CreateClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Headers.Add("x-hotel-token", loginString);
            /*httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(secrets, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Encoding.UTF8,
                "application/json");*/
            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri = new Uri($"{ServerApiConfig.BaseUrl}/hotelInfo");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
            if (httpResponse.IsSuccessStatusCode)
            {/*
                CookieOptions options = new CookieOptions();
                Response.Cookies.Append(
                    HotelTokenCookieDefaults.AuthCookieName,
                    loginString,
                    options);*/
                return Redirect("/profile");
            }
            //Console.WriteLine($"Status code: {httpResponse.StatusCode}\n{await httpResponse.Content.ReadAsStringAsync()}");
            string serverError = null;
            try
            {
                LogInError error = JsonSerializer.Deserialize<LogInError>(
                    await httpResponse.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );
                serverError = error.Error;
            }
            catch (JsonException)
            {
                serverError = "500: Internal server error";
            }/*
            LogInViewModel viewModel = new LogInViewModel()
            {
                ServerLogInError = serverError
            };
            return View(viewModel);*/
            //return Redirect("/profile");
            return View("~/Views/Login/Index.cshtml");
        }

        [HttpGet("/logout")]
        public IActionResult LogOut()
        {
            this.Response.Cookies.Delete(HotelTokenCookieDefaults.AuthCookieName);
            return Redirect("/");
        }
        class ClientSecrets
        {
            public ClientSecrets(string loginString)
            {
                LoginString = loginString;
            }
            public string LoginString { get; }
        }

        class LogInError
        {
            public string Error { get; set; }
        }
    }
}
