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

namespace Client_Module.Authentication
{
    public class ClientCookieTokenManager : IClientCookieTokenManager
    {
        private IClientInfoAccessor _clientInfoAccessor;
        public ClientCookieTokenManager(IClientInfoAccessor clientInfoAccessor)
        {
            _clientInfoAccessor = clientInfoAccessor;
        }

        public ClaimsPrincipal CreatePrincipal(ClientInfo clientToken)
        {
            if(clientToken == null)
            {
                throw new ArgumentNullException("clientToken");
            }
            var claims = new[]
            {
                new Claim("name", clientToken.Name),
                new Claim("surname", clientToken.Surname),
                new Claim("email", clientToken.Email),
                new Claim("username", clientToken.Username)
            };
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }

        public ClientInfo ValidateCookieToken(string cookieToken, out string validationError)
        {
            if(string.IsNullOrEmpty(cookieToken))
            {
                validationError = "Client cookie token must be a non-empy string";
                return null;
            }
            ClientInfo clientInfo = _clientInfoAccessor.GetClientInfo(cookieToken, out validationError);
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
