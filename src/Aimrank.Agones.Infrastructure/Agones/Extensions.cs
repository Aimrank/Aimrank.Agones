using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Agones.Infrastructure.Agones
{
    internal static class Extensions
    {
        public static IServiceCollection AddAgones(this IServiceCollection services)
        {
            services.AddSingleton<IAgonesService, AgonesService>();
            return services;
        }
    }
}