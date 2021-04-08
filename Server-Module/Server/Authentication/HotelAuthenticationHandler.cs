using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Database;
using Server.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Server.Authentication
{
    public class HotellTokenScheme
        : AuthenticationHandler<HotellTokenSchemeOptions>
    {
        private IHotellTokenDataAcess hotellTokenDataAcess;
        private HotellTokenSchemeOptions _options;
        public HotellTokenScheme(
            IOptionsMonitor<HotellTokenSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            hotellTokenDataAcess = (IHotellTokenDataAcess)Context.RequestServices.GetService(typeof(IHotellTokenDataAcess));
           // this.hotellTokenDataAcess = hotellTokenDataAcess;
            _options = options.CurrentValue;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string myToken;
            try
            {
                myToken = this.Context.Request.Headers["x-hotel-token"][0];
            }
            catch(Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e));
            }

            int? wynik = hotellTokenDataAcess.GetMyId(myToken);
            if ( !wynik.HasValue )
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var claims = new[] { new Claim("clientToken", myToken) };
            var identity = new ClaimsIdentity(claims, HotellTokenDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, HotellTokenDefaults.AuthenticationScheme);
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

    public class HotellTokenSchemeOptions
        : AuthenticationSchemeOptions
    {

    }

    public static class HotellTokenDefaults
    {
        public static string AuthenticationScheme { get; } = "HotellTokenScheme";
        public static string AuthCookieName { get; set; } = "hotellTokenASPNET";
    }
}
