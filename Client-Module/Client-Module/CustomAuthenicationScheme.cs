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
using Microsoft.AspNetCore.Mvc.Routing;

namespace Client_Module
{
    public class ClientTokenCookieScheme
        : AuthenticationHandler<ClientTokenCookieSchemeOptions>
    {
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
            //LinkGenerator urlGenerator = Context.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator;
            //Console.WriteLine($"{urlGenerator.GetPathByAction("LogIn", "Client")}");
            if (!Request.Cookies.ContainsKey(ClientTokenCookieDefaults.AuthCookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string clientToken = Request.Cookies[ClientTokenCookieDefaults.AuthCookieName];
            var claims = new[] { new Claim("clientToken", clientToken) };
            var identity = new ClaimsIdentity(claims, ClientTokenCookieDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, ClientTokenCookieDefaults.AuthenticationScheme);
            //Console.WriteLine("User logged in");

            //Context.RequestServices.GetService(typeof(IHotelCredentialsDataAccess))

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

    public class ClientTokenCookieSchemeOptions
        : AuthenticationSchemeOptions
    { 

    }

    public static class ClientTokenCookieDefaults
    {
        public static string AuthenticationScheme { get; } = "ClientTokenCookieScheme";
        public static string AuthCookieName { get; set; } = "clientTokenCookie";
    }
}
