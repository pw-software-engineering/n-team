using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
                HttpClient httpClient = _httpClientFactory.CreateClient(nameof(DefaultHttpClient));
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
