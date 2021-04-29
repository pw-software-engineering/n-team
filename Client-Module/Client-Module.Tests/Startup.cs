using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ClientModule.Tests.Authentication;
using Client_Module.Authentication;
using Moq;

namespace ClientModule.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<ClientCookieTokenManager>();
        }
    }
}
