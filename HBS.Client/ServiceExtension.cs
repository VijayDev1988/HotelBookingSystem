using HBS.Client.Services.Concreate;
using HBS.Identity.Contexts;
using HBS.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client
{
    public static class ServiceExtension
    {
        public static void AddGoogleOauth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "13373888262-6di47erd7te0hs3bm4dp2ollg1fo6pif.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });
        }

        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<RoomBookingServices>>();
            services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), logger);

        }
    }
}
