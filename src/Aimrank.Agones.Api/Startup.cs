using Aimrank.Agones.Api.Middleware;
using Aimrank.Agones.Core.Services;
using Aimrank.Agones.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aimrank.Agones.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddExceptionsHandler();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseInfrastructure();
            app.UseExceptionsHandler();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Aimrank.Agones"));
            });
            
            lifetime.ApplicationStopping.Register(async () =>
            {
                var manager = app.ApplicationServices.GetRequiredService<IServerProcessManager>();
                await manager.DisconnectAsync();
            });
        }
    }
}