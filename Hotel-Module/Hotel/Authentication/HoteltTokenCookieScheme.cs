using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hotel_Module.Authentication
{
    public class HoteltTokenCookieScheme
        : AuthenticationHandler<HotelTokenCookieSchemeOptions>
    {
        private HotelTokenCookieSchemeOptions _options;
        private IHotelCookieTokenManager _cookieTokenManager;
        public HoteltTokenCookieScheme(
            IHotelCookieTokenManager cookieTokenManager,
            IOptionsMonitor<HotelTokenCookieSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options.CurrentValue;
            _cookieTokenManager = cookieTokenManager;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine($"{Guid.NewGuid()} - AUTHENTICATE");
            if (!Request.Cookies.ContainsKey(HotelTokenCookieDefaults.AuthCookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string clientCookieToken = Request.Cookies[HotelTokenCookieDefaults.AuthCookieName];

            HotelInfo clientInfo = _cookieTokenManager.ValidateCookieToken(clientCookieToken, out string validationError);
            if (clientInfo == null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var principal = _cookieTokenManager.CreatePrincipal(clientInfo);
            var ticket = new AuthenticationTicket(principal, HotelTokenCookieDefaults.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode}");
            LinkGenerator urlGenerator = Context.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator;
            //Console.WriteLine($"{urlGenerator.GetPathByAction("LogIn", "Client")}");
            Context.Response.Redirect(urlGenerator.GetPathByAction("LogIn", "Client"));
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"FORBIDDEN: {Response.StatusCode}");
            //Response.Redirect(_options.ChallangeRedirectUrl, false);
            //return Task.CompletedTask;
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
