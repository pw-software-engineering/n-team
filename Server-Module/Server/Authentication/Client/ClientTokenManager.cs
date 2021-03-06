﻿using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Database.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Server.Authentication.Client
{
    public interface IClientTokenManager
    {
        ClientToken ParseTokenHeader(StringValues tokenHeader, out string parseError);
        bool ValidateToken(ClientToken clientToken, out string validationError);
        ClaimsPrincipal CreatePrincipal(ClientToken clientToken);
    }

    public static class ClientTokenManagerOptions
    {
        public const string ClientIdClaimName = "id";
    }

    public class ClientTokenManager : IClientTokenManager
    {
        private readonly IClientTokenDataAccess _tokenDataAccess;
        public ClientTokenManager(IClientTokenDataAccess tokenDataAccess)
        {
            _tokenDataAccess = tokenDataAccess;
        }

        public ClaimsPrincipal CreatePrincipal(ClientToken clientToken)
        {
            if (clientToken == null)
            {
                throw new ArgumentNullException("clientToken");
            }
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClientTokenManagerOptions.ClientIdClaimName, clientToken.ID.ToString()));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }

        public ClientToken ParseTokenHeader(StringValues tokenHeader, out string parseError)
        {
            if (StringValues.IsNullOrEmpty(tokenHeader))
            {
                parseError = $"Missing {ClientTokenDefaults.TokenHeaderName} header value";
                return null;
            }
            if (tokenHeader.Count > 1)
            {
                parseError = $"Header {ClientTokenDefaults.TokenHeaderName} must include only 1 value";
                return null;
            }
            string tokenStr = tokenHeader[0];
            ClientToken clientToken;
            try
            {
                clientToken = JsonConvert.DeserializeObject<ClientToken>(
                    tokenStr,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                //clientToken = JsonSerializer.Deserialize<ClientToken>(
                //    tokenStr,
                //    new JsonSerializerOptions()
                //    {
                //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //    }
                //);
            }
            catch (JsonException)
            {
                parseError = $"Token provided in {ClientTokenDefaults.TokenHeaderName} is malformed";
                return null;
            }
            parseError = null;
            return clientToken;
        }

        public bool ValidateToken(ClientToken clientToken, out string validationError)
        {
            if(clientToken == null)
            {
                throw new ArgumentNullException("clientToken");
            }
            if (!_tokenDataAccess.CheckIfClientExists(clientToken.ID))
            {
                validationError = $"Token provided in {ClientTokenDefaults.TokenHeaderName} contains illegal value in clientID property";
                return false;
            }
            validationError = null;
            return true;
        }
    }
}
