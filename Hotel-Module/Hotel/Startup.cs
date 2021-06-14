using Hotel.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hotel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ServerApiConfig.TokenHeaderName = Configuration.GetSection("ServerApiConfig").GetValue<string>("TokenHeaderName");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<DefaultHttpClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetSection("ServerApiConfig").GetValue<string>("BaseUrl"));
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
#if PRODUCTION
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = new Microsoft.AspNetCore.Http.PathString("/hotel");
                await next();
            });
#endif

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
