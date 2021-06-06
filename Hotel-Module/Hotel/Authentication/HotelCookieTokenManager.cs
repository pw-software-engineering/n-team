using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel_Module.Authentication
{
    public class HotelCookieTokenManager : IHotelCookieTokenManager
    {
        public ClaimsPrincipal CreatePrincipal(string auth)
        {
            if(auth == null)
            {
                throw new ArgumentNullException("hotelToken");
            }
            var claims = new[]
            {
                new Claim("authString", auth),
            };
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
    }

    public class HotelCookieTokenManagerOptions
    {
        public const string AuthStringClaimType = "authString";
    }
}
