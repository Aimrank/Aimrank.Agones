using Aimrank.Agones.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    internal static class Extensions
    {
        public static IServiceCollection AddCSGOServer(this IServiceCollection services)
        {
            services.AddSingleton<IServerEventMapper, ServerEventMapper>();
            services.AddSingleton<IServerProcessManager, ServerProcessManager>();
            return services;
        }

        public static IApplicationBuilder UseCSGOServer(this IApplicationBuilder builder)
        {
            var processManager = builder.ApplicationServices.GetRequiredService<IServerProcessManager>();
            processManager.StartServerAsync().GetAwaiter().GetResult();
            return builder;
        }
    }
}