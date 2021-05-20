using Aimrank.Agones.Core.Events;
using Aimrank.Agones.Infrastructure.Agones;
using Aimrank.Agones.Infrastructure.CSGO;
using Aimrank.Agones.Infrastructure.Cluster;
using Aimrank.Agones.Infrastructure.EventBus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Agones.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(IEvent));

            services.AddAgones();
            services.AddCluster(configuration);
            services.AddEventBus(configuration);
            services.AddCSGOServer();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            builder.UseCSGOServer();
            return builder;
        }
    }
}