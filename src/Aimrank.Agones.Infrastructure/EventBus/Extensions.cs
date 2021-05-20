using Aimrank.Agones.Core.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Agones.Infrastructure.EventBus
{
    internal static class Extensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));
            services.AddSingleton<IEventsDispatcher, RabbitMQEventsDispatcher>();
            
            return services;
        }
    }
}