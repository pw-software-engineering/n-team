using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hotel
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
            services.AddHttpClient<DefaultHttpClient>(c =>
            {
                c.BaseAddress = new Uri($"{ServerApiConfig.BaseUrl.TrimEnd('/')}/");
            });
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddAuthentication(HotelTokenCookieDefaults.AuthenticationScheme).AddScheme<HotelTokenCookieSchemeOptions, HotelTokenCookieScheme>(
                HotelTokenCookieDefaults.AuthenticationScheme, (HotelTokenCookieSchemeOptions opt) =>
                {
                    opt.ClaimsIssuer = "localhost";
                });
            //services.AddSingleton<Hotel_Module.Authentication.IHotelInfoAccessor, Hotel_Module.Authentication.HotelInfoAccessor>();
            services.AddSingleton<IHotelCookieTokenManager, HotelCookieTokenManager>();
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
            #ifndef PRODUCTION
            app.UseHttpsRedirection();
            #endif
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
