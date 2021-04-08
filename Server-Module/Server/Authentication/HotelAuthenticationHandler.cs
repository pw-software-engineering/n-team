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
    public class HotelTokenScheme
        : AuthenticationHandler<HotelTokenSchemeOptions>
    {
        private IHotelTokenDataAccess hotelTokenDataAcess;
        private HotelTokenSchemeOptions _options;
        public HotelTokenScheme(IHotelTokenDataAccess hotelTokenDataAcess,
            IOptionsMonitor<HotelTokenSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            this.hotelTokenDataAcess = hotelTokenDataAcess;
           // this.hotellTokenDataAcess = hotellTokenDataAcess;
            _options = options.CurrentValue;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string hotelToken;
            try
            {
                hotelToken = this.Context.Request.Headers["x-hotel-token"][0];
            }
            catch(Exception)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
                //return Task.FromResult(AuthenticateResult.Fail(e));
            }

            int? hotelId = hotelTokenDataAcess.GetHotelIdFromToken(hotelToken);
            if ( !hotelId.HasValue )
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var claims = new[] { new Claim("clientToken", hotelId.Value.ToString()) };
            var identity = new ClaimsIdentity(claims, HotelTokenDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, HotelTokenDefaults.AuthenticationScheme);
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

    public class HotelTokenSchemeOptions
        : AuthenticationSchemeOptions
    {

    }

    public static class HotelTokenDefaults
    {
        public static string AuthenticationScheme { get; } = "HotellTokenScheme";
    }
}
