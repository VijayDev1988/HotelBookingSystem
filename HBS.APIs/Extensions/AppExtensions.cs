using HBS.APIs.Middlewares;
using HBS.APIs.Services;
using HBS.Application.Features;
using HBS.Application.Interfaces;
using HBS.Application.Interfaces.Repositories;
using HBS.Persistence.Contexts;
using HBS.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HBS.APIs v1");
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static void DependencyInjectionExtension(this IServiceCollection services)
        {
            services.AddScoped<IRoomQuery, RoomsQuerys>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped(typeof(IGenericRepositoryAsync<>), (typeof(GenericRepositoryAsync<>)));
            services.AddScoped<IRoomBookingRepository, RoomBookingRepositoryAsync>();
            services.AddScoped<IBookingDetailsRepository, BookingDetailsRepository>();
        }


        /// <summary>
        /// Cors Middleware.
        /// </summary>
        /// <param name="app"></param>
        public static void EnableCors(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                         .AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
        }
    }
}
