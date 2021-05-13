using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client_Module.Authentication
{
    public interface IClientInfoAccessor
    {
        public ClientInfo GetClientInfo(string cookieToken, out string serverError);
    }
    public class ClientInfoAccessor : IClientInfoAccessor
    {
        private IHttpClientFactory _httpClientFactory;

        public ClientInfoAccessor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public ClientInfo GetClientInfo(string cookieToken, out string serverError)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("default-server-api");
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add(ServerApiConfig.TokenHeaderName, cookieToken);
            httpRequest.RequestUri = new Uri($"{ServerApiConfig.BaseUrl}/client");
            HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                serverError = response.Content.ReadAsStringAsync().Result;
                return null;
            }
            string responseJSON = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(responseJSON);
            ClientInfo clientInfo = null;
            try
            {
                clientInfo = JsonConvert.DeserializeObject<ClientInfo>(
                    responseJSON,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
            catch(JsonException)
            {
                serverError = "Invalid client info JSON object format";
            }
            serverError = null;
            return clientInfo;
        }
    }
}
