using Client_Module.Controllers;
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
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Client_Module
{
    public class ClientTokenCookieScheme
        : AuthenticationHandler<ClientTokenCookieSchemeOptions>
    {
        private readonly string _cookieName = "clientTokenCookieASPNET";
        private ClientTokenCookieSchemeOptions _options;
        public ClientTokenCookieScheme(
            IOptionsMonitor<ClientTokenCookieSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options.CurrentValue;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Console.WriteLine("AUTHENTICATE");
            if (!Request.Cookies.ContainsKey(_cookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string clientToken = Request.Cookies[_cookieName];
            var claims = new[] { new Claim("clientToken", clientToken) };
            var identity = new ClaimsIdentity(claims, ClientTokenCookieDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, ClientTokenCookieDefaults.AuthenticationScheme);

            //this.Context.Request.RouteValues

            

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode}");
            //Response.Redirect(_options.ChallangeRedirectUrl, false);
            //return Task.CompletedTask;
            return base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"FORBIDDEN: {Response.StatusCode}");
            //Response.Redirect(_options.ChallangeRedirectUrl, false);
            //return Task.CompletedTask;
            return base.HandleForbiddenAsync(properties);
        }
    }

    public class ClientTokenCookieSchemeOptions
        : AuthenticationSchemeOptions
    {
        public string ChallangeRedirectUrl { get; set; } = "/Home/Page";
    }

    public static class ClientTokenCookieDefaults
    {
        public static string AuthenticationScheme { get; } = "ClientTokenCookieScheme";
    }
}
