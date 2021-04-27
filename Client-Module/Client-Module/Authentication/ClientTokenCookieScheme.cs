using Client_Module.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Client_Module.Authentication
{
    public class ClientTokenCookieScheme
        : AuthenticationHandler<ClientTokenCookieSchemeOptions>
    {
        private ClientTokenCookieSchemeOptions _options;
        private IClientCookieTokenManager _cookieTokenManager;
        public ClientTokenCookieScheme(
            IClientCookieTokenManager cookieTokenManager,
            IOptionsMonitor<ClientTokenCookieSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options.CurrentValue;
            _cookieTokenManager = cookieTokenManager;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine($"{Guid.NewGuid()} - AUTHENTICATE");
            if (!Request.Cookies.ContainsKey(ClientTokenCookieDefaults.AuthCookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string clientCookieToken = Request.Cookies[ClientTokenCookieDefaults.AuthCookieName];

            ClientInfo clientInfo = _cookieTokenManager.ValidateCookieToken(clientCookieToken, out string validationError);
            if (clientInfo == null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var principal = _cookieTokenManager.CreatePrincipal(clientInfo);
            var ticket = new AuthenticationTicket(principal, ClientTokenCookieDefaults.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"CHALLANGE: {Response.StatusCode}");
            LinkGenerator urlGenerator = Context.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator;
            //Console.WriteLine($"{urlGenerator.GetPathByAction("LogIn", "Client")}");
            Context.Response.Redirect(urlGenerator.GetPathByAction("LogIn", "Client"));
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Console.WriteLine($"FORBIDDEN: {Response.StatusCode}");
            //Response.Redirect(_options.ChallangeRedirectUrl, false);
            //return Task.CompletedTask;
            return base.HandleForbiddenAsync(properties);
        }
    }

    public class ClientTokenCookieSchemeOptions
        : AuthenticationSchemeOptions
    { 

    }

    public static class ClientTokenCookieDefaults
    {
        public const string AuthenticationScheme = "ClientTokenCookieScheme";
        public const string AuthCookieName = "clientTokenCookie";
    }
}
