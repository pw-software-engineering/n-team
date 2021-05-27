using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Server.Authentication.Hotel;
using Server.Authentication.Client;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Server.Database.Models;
using Server.ViewModels;
using Server.Services.Hotel;
using Server.Services.Client;
using Server.Database.DataAccess.Hotel;
using Server.Database.DataAccess.Client;
using Server.AutoMapper;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
                opts.AddProfile(new HotelAutoMapperProfile());
            },
            typeof(Startup));

            services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IClientAccountService, ClientAccountService>();
            services.AddTransient<IOfferDataAccess, OfferDataAccess>();
            services.AddTransient<IClientDataAccess, ClientDataAccess>();
            services.AddTransient<IRoomDataAccess, RoomDataAccess>();
            services.AddTransient<IHotelTokenDataAccess, HotelTokenDataAccess>();
            services.AddTransient<IHotelAccountDataAccess, HotelAccountDataAccess>();
            services.AddTransient<IHotelAccountService, HotelAccountService>();
            services.AddTransient<IHotelSearchDataAccess, HotelSearchDataAccess>();
            services.AddTransient<IHotelSearchService, HotelSearchService>();
            services.AddTransient<IOfferSearchDataAccess, OfferSearchDataAccess>();
            services.AddTransient<IOfferSearchService, OfferSearchService>();
            services.AddTransient<Database.DataAccess.Client.IReservationDataAccess, Database.DataAccess.Client.ReservationDataAccess>();
            services.AddTransient<Services.Client.IReservationService, Services.Client.ReservationService>();
            services.AddTransient<Database.DataAccess.Hotel.IReservationDataAccess, Database.DataAccess.Hotel.ReservationDataAccess>();
            services.AddTransient<Services.Hotel.IReservationService, Services.Hotel.ReservationService>();
            services.AddTransient<IOfferRoomDataAccess, OfferRoomDataAccess>();
            services.AddTransient<IOfferRoomService, OfferRoomService>();

            services.AddTransient<IClientTokenManager, ClientTokenManager>();
            services.AddTransient<IClientTokenDataAccess, ClientTokenDataAccess>();

            services.AddDbContext<ServerDbContext>(options =>           
                options.UseSqlServer(Configuration.GetConnectionString("ServerDBContext")));
            //services.AddAuthentication("HotellBasic").AddScheme<HotellTokenSchemeOptions, HotellTokenScheme>("HotellBasic", null);
            services.AddAuthentication()
                .AddScheme<HotelTokenSchemeOptions, HotelTokenScheme>(
                HotelTokenDefaults.AuthenticationScheme, 
                (HotelTokenSchemeOptions options) => { 
                    options.ClaimsIssuer = "HotelBasic"; 
                })
                .AddScheme<ClientTokenSchemeOptions, ClientTokenScheme>(
                ClientTokenDefaults.AuthenticationScheme,
                (ClientTokenSchemeOptions options) =>
                {
                    options.ClaimsIssuer = "ClientToken";
                });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add(
            //        "Access-Control-Allow-Origin", 
            //        new StringValues("*"));
            //    context.Response.Headers.Add(
            //        "Access-Control-Allow-Headers",
            //        new StringValues(new string[] { ClientTokenDefaults.TokenHeaderName, HotelTokenDefaults.TokenHeaderName }));
            //    context.Response.Headers.Add(
            //        "Access-Control-Allow-Methods",
            //        new StringValues(new string[] { "PUT", "DELETE", "PATCH", "GET", "POST" }));
            //    await next();
            //});

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
