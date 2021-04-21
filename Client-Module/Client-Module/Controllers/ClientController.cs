using Client_Module.Models;
using Client_Module.ViewsTagID.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client_Module.Controllers
{
    public class ClientController : Controller
    {
        private ILogger<ClientController> _logger;
        private IHttpClientFactory _httpClientFactory;
        public ClientController(ILogger<ClientController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("/login")]
        public IActionResult LogIn()
        {
            return View(new LogInViewModel());
        }

        [HttpGet("/logout")]
        public IActionResult LogOut()
        {
            this.Response.Cookies.Delete(ClientTokenCookieDefaults.AuthCookieName);
            return Redirect("/");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            Console.WriteLine($"{username} | {password}");
            ClientSecrets secrets = new ClientSecrets(username, password);
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(secrets, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), 
                Encoding.UTF8, 
                "application/json");
            httpRequest.Method = new HttpMethod("POST");
            httpRequest.RequestUri = new Uri(ServerApiConfig.BaseUrl + "/api/client/login");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
            if(httpResponse.StatusCode == HttpStatusCode.OK)
            {
                CookieOptions options = new CookieOptions();
                Response.Cookies.Append(
                    ClientTokenCookieDefaults.AuthCookieName, 
                    await httpResponse.Content.ReadAsStringAsync(), 
                    options);
                return Redirect("/");
            }
            //Console.WriteLine($"Status code: {httpResponse.StatusCode}\n{await httpResponse.Content.ReadAsStringAsync()}");
            LogInError error = JsonSerializer.Deserialize<LogInError>(
                await httpResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
            LogInViewModel viewModel = new LogInViewModel()
            {
                ServerLogInError = error.Error
            };
            return View(viewModel);
        }

        [HttpGet("/account")]
        [Authorize(AuthenticationSchemes = "ClientTokenCookieScheme")]
        public IActionResult Account()
        {
            ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavAccountBtnID;
            return View();
        }
    }

    class ClientSecrets
    {
        public ClientSecrets(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public string Login { get; }
        public string Password { get; }
    }

    class LogInError
    {
        public string Error { get; set; }
    }
}
