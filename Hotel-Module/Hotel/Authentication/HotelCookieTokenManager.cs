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
        private IHotelInfoAccessor _hotelInfoAccessor;
        public HotelCookieTokenManager(IHotelInfoAccessor hotelInfoAccessor)
        {
            _hotelInfoAccessor = hotelInfoAccessor;
        }

        public ClaimsPrincipal CreatePrincipal(HotelInfo hotel)
        {
            if(hotel == null)
            {
                throw new ArgumentNullException("hotelToken");
            }
            var claims = new[]
            {
                new Claim("authString", hotel.hotelName),
            };
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }

        public HotelInfo ValidateCookieToken(string cookieToken, out string validationError)
        {
            if(string.IsNullOrEmpty(cookieToken))
            {
                validationError = "Client cookie token must be a non-empy string";
                return null;
            }
            HotelInfo clientInfo = _hotelInfoAccessor.GetHotelInfo(cookieToken, out validationError);
            if (clientInfo == null)
            {
                if(validationError == null)
                {
                    validationError = "Unexpected error - clientInfo contract is not fulfilled";
                }
                return null;
            }
            validationError = null;
            return clientInfo;
        }
    }
}
