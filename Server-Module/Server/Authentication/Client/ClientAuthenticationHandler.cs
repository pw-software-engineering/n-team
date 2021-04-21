using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Server.Database;
using Server.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Authentication.Client
{
    public class ClientTokenScheme
        : AuthenticationHandler<ClientTokenSchemeOptions>
    {
        private IClientTokenManager _tokenManager;
        private ClientTokenSchemeOptions _options;

        private string _errorString = null;

        public ClientTokenScheme(
            IClientTokenManager tokenManager,
            IOptionsMonitor<ClientTokenSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _tokenManager = tokenManager;
            _options = options.CurrentValue;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues tokenHeader = Context.Request.Headers[ClientTokenDefaults.TokenHeaderName];
            ClientToken clientToken = null;
            try
            {
                if((clientToken = _tokenManager.ParseTokenHeader(tokenHeader, out _errorString)) == null)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }
            }
            catch(Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e));
            }
            if(!_tokenManager.ValidateToken(clientToken, out _errorString))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var ticket = new AuthenticationTicket(
                _tokenManager.CreatePrincipal(clientToken),
                HotelTokenDefaults.AuthenticationScheme
            );
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode}");
            Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            Context.Response.ContentType = "application/json";
            await Context.Response.WriteAsync(_errorString);
            return;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"FORBIDDEN: {Response.StatusCode}");
            //Response.Redirect(_options.ChallangeRedirectUrl, false);
            //return Task.CompletedTask;
            return base.HandleForbiddenAsync(properties);
        }
    }

    public static class ClientTokenDefaults
    {
        public const string AuthenticationScheme = "HotelTokenScheme";
        public const string TokenHeaderName = "x-client-token";
    }

    public class ClientTokenSchemeOptions : AuthenticationSchemeOptions
    {
        
    }
}
