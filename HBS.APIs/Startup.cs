using CleanArchitecture.WebAPI.WebApi.Extensions;
using HBS.APIs.Extensions;
using HBS.Application;
using HBS.Application.Features;
using HBS.Application.Interfaces;
using HBS.Identity;
using HBS.Persistence;
using HBS.Persistence.Contexts;
using HBS.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HBS.APIs
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
            services.AddApplicationLayer();
            services.DependencyInjectionExtension();
            services.AddIdentityInfrastructure(Configuration);
            services.AddPersistenceInfrastructure(Configuration);
            services.AddSharedInfrastructure(Configuration);
            services.AddSwaggerExtension();
            services.AddApiVersioningExtension();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddCors();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "13373888262-6di47erd7te0hs3bm4dp2ollg1fo6pif.apps.googleusercontent.com";
                options.ClientSecret = "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";
                // options.CallbackPath = ""
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnableCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwaggerExtension();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
