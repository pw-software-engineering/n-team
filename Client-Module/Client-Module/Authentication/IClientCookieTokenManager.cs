using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client_Module.Authentication
{
    public interface IClientCookieTokenManager
    {
        ClientInfo ValidateCookieToken(string cookieToken, out string validationError);
        ClaimsPrincipal CreatePrincipal(ClientInfo clientToken);
    }
}
