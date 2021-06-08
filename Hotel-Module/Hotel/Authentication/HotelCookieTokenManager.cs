using System;
using System.Security.Claims;

namespace Hotel.Authentication
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
