using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace Client_Module
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ServerApiConfig.BaseUrl = Configuration.GetSection("ServerApiConfig").GetValue<string>("BaseUrl");
            ServerApiConfig.TokenHeaderName = Configuration.GetSection("ServerApiConfig").GetValue<string>("TokenHeaderName");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddControllersWithViews();
            services.AddHttpClient();
            //services.AddHttpClient("default-server-api", (HttpClient client) =>
            //{
            //    client.BaseAddress = new Uri(ServerApiConfig.BaseUrl.TrimEnd('/') + '/');
            //});
            services.AddAuthentication(ClientTokenCookieDefaults.AuthenticationScheme)
                .AddScheme<ClientTokenCookieSchemeOptions, ClientTokenCookieScheme>(
                ClientTokenCookieDefaults.AuthenticationScheme,
                (ClientTokenCookieSchemeOptions options) =>
                {
                    options.ClaimsIssuer = "localhost";
                });
            services.AddSingleton<IClientInfoAccessor, ClientInfoAccessor>();
            services.AddSingleton<IClientCookieTokenManager, ClientCookieTokenManager>();
            //services.AddTransient<IViewRenderService, ViewRenderService>();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("def", (AuthorizationPolicyBuilder b) => b.)
            //    options.AddPolicy("Token", policy =>
            //        policy.Requirements.Add(new TokenRequirement()));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #if !PRODUCTION
            app.UseHttpsRedirection();
            #endif
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}
