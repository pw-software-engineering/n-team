using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Server.Authentication.Client
{
    public class ClientTokenScheme
        : AuthenticationHandler<ClientTokenSchemeOptions>
    {
        private IClientTokenManager _tokenManager;

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
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues tokenHeader = Context.Request.Headers[ClientTokenDefaults.TokenHeaderName];
            ClientToken clientToken = null;
            try
            {
                if ((clientToken = _tokenManager.ParseTokenHeader(tokenHeader, out _errorString)) == null)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }
            }
            catch (Exception e)
            {
                return Task.FromResult(AuthenticateResult.Fail(e));
            }
            if (!_tokenManager.ValidateToken(clientToken, out _errorString))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var ticket = new AuthenticationTicket(
                _tokenManager.CreatePrincipal(clientToken),
                ClientTokenDefaults.AuthenticationScheme
            );
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode} | {_errorString}");
            IServiceResult result = new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView(_errorString));
            RouteData routeData = new RouteData(Context.Request.RouteValues);
            ActionDescriptor actionDescriptor = new ActionDescriptor();
            await result.ExecuteResultAsync(new ActionContext(Context, routeData, actionDescriptor));
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
        public const string AuthenticationScheme = "ClientTokenScheme";
        public const string TokenHeaderName = "x-client-token";
    }

    public class ClientTokenSchemeOptions : AuthenticationSchemeOptions
    {
        
    }
}
