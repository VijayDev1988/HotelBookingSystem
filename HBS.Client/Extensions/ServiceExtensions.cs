using HBS.Client.Services.Concreate;
using HBS.Client.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HBS.Client.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddExtensions(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void AddDependcyInjection(this IServiceCollection services)
        {
            services.AddHttpClient<IRoomBookingServices, RoomBookingServices>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44377/");
            });

            services.AddHttpClient<IAccountServices, AccountServices>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44377/");
            });
        }
    }
}
