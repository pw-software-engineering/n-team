using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Authentication;
using Server.Database;
using Server.Database.DataAccess;
using Server.Services.OfferService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IDataAccess, DataAccess>();
            services.AddTransient<IHotelTokenDataAccess, HotelTokenDataAccess>();
            services.AddDbContext<ServerDbContext>(options =>           
                options.UseSqlServer(Configuration.GetConnectionString("ServerDBContext"))); 
			//services.AddAuthentication("HotellBasic").AddScheme<HotellTokenSchemeOptions, HotellTokenScheme>("HotellBasic", null);
            services.AddAuthentication().AddScheme<HotelTokenSchemeOptions, HotelTokenScheme>(HotelTokenDefaults.AuthenticationScheme, (HotelTokenSchemeOptions options) => { options.ClaimsIssuer = "HotelBasic"; });
            services.AddHttpContextAccessor();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
