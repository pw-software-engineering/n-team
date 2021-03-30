using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Server.Authentication
{
    public class HotellTokenCookieScheme
        : AuthenticationHandler<HotellTokenCookieSchemeOptions>
    {
        private HotellTokenCookieSchemeOptions _options;
        public HotellTokenCookieScheme(
            IOptionsMonitor<HotellTokenCookieSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options.CurrentValue;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine("AUTHENTICATE");
            //LinkGenerator urlGenerator = Context.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator;
            //Console.WriteLine($"{urlGenerator.GetPathByAction("LogIn", "Client")}");
            if (!Request.Cookies.ContainsKey(HotellTokenCookieDefaults.AuthCookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string clientToken = Request.Cookies[HotellTokenCookieDefaults.AuthCookieName];
            var claims = new[] { new Claim("clientToken", clientToken) };
            var identity = new ClaimsIdentity(claims, HotellTokenCookieDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, HotellTokenCookieDefaults.AuthenticationScheme);
            Console.WriteLine("User logged in");
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

    public class HotellTokenCookieSchemeOptions
        : AuthenticationSchemeOptions
    {

    }

    public static class HotellTokenCookieDefaults
    {
        public static string AuthenticationScheme { get; } = "HotellTokenCookieScheme";
        public static string AuthCookieName { get; set; } = "hotellTokenCookieASPNET";
    }
}
