using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Http;

namespace Hotel.Authentication
{
    public class HotelTokenCookieScheme
        : AuthenticationHandler<HotelTokenCookieSchemeOptions>
    {
        private HotelTokenCookieSchemeOptions _options;
        private IHotelCookieTokenManager _cookieTokenManager;
        private IHttpClientFactory _httpClientFactory;
        public HotelTokenCookieScheme(
            IHttpClientFactory httpClientFactory,
            IHotelCookieTokenManager cookieTokenManager,
            IOptionsMonitor<HotelTokenCookieSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options.CurrentValue;
            _cookieTokenManager = cookieTokenManager;
            _httpClientFactory = httpClientFactory;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine($"{Guid.NewGuid()} - AUTHENTICATE");
            if (!Request.Cookies.ContainsKey(HotelTokenCookieDefaults.AuthCookieName))
            {
                return AuthenticateResult.NoResult();
            }
            string authString = Request.Cookies[HotelTokenCookieDefaults.AuthCookieName];

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient(nameof(DefaultHttpClient));
                httpClient.DefaultRequestHeaders.Add(ServerApiConfig.TokenHeaderName, authString);
                HttpResponseMessage httpResponse = await httpClient.GetAsync("rooms?pageNumber=1&pageSize=1");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return AuthenticateResult.NoResult();
                }
            }
            catch
            {
                return AuthenticateResult.NoResult();
            }

            var principal = _cookieTokenManager.CreatePrincipal(authString);
            var ticket = new AuthenticationTicket(principal, HotelTokenCookieDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode}");
            LinkGenerator urlGenerator = Context.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator;
            Context.Response.Redirect(urlGenerator.GetPathByAction("Login", "Login"));
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"FORBIDDEN: {Response.StatusCode}");
            return base.HandleForbiddenAsync(properties);
        }
    }

    public class HotelTokenCookieSchemeOptions
        : AuthenticationSchemeOptions
    { 

    }

    public static class HotelTokenCookieDefaults
    {
        public const string AuthenticationScheme = "HoteltTokenCookieScheme";
        public const string AuthCookieName = "clientTokenCookie";
    }
}
