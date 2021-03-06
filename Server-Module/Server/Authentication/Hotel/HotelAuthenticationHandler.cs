﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Database.DataAccess.Hotel;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Server.Authentication.Hotel
{
    public class HotelTokenScheme
        : AuthenticationHandler<HotelTokenSchemeOptions>
    {
        private IHotelTokenDataAccess _hotelTokenDataAcess;
        public HotelTokenScheme(IHotelTokenDataAccess hotelTokenDataAcess,
            IOptionsMonitor<HotelTokenSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _hotelTokenDataAcess = hotelTokenDataAcess;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string hotelToken;
            try
            {
                hotelToken = Context.Request.Headers[HotelTokenDefaults.TokenHeaderName][0];
            }
            catch(Exception)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
                //return Task.FromResult(AuthenticateResult.Fail(e));
            }

            int? hotelId = _hotelTokenDataAcess.GetHotelIdFromToken(hotelToken);
            if ( !hotelId.HasValue )
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var claims = new[] { new Claim(HotelTokenManagerOptions.HotelIdClaimName, hotelId.Value.ToString()) };
            var identity = new ClaimsIdentity(claims, HotelTokenDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, HotelTokenDefaults.AuthenticationScheme);
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
        public const string AuthenticationScheme = "HotelTokenScheme";
        public const string TokenHeaderName = "x-hotel-token";
    }
}
