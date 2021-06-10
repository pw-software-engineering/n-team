using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotel.Authentication
{
    public interface IHotelCookieTokenManager
    {
        ClaimsPrincipal CreatePrincipal(string auth);
    }
}
