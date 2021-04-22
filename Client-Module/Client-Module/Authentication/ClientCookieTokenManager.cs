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
        private IHttpClientFactory _httpClientFactory;
        public ClientCookieTokenManager(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public ClaimsPrincipal CreatePrincipal(ClientInfo clientToken)
        {
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
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = new HttpMethod("GET");
            httpRequest.Headers.Add(ServerApiConfig.TokenHeaderName, cookieToken);
            httpRequest.RequestUri = new Uri(ServerApiConfig.BaseUrl + "/client");
            HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                validationError = response.Content.ReadAsStringAsync().Result;
                return null;
            }
            string responseJSON = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(responseJSON);
            ClientInfo clientInfo = JsonConvert.DeserializeObject<ClientInfo>(
                responseJSON,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            if (clientInfo == null)
            {
                validationError = "Unexpected error - clientInfo contract is not fulfilled";
                return null;
            }
            validationError = null;
            return clientInfo;
        }
    }
}
