using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aimrank.Agones.Api.Middleware
{
    public static class ExceptionsHandlerExtensions
    {
        public static IServiceCollection AddExceptionsHandler(this IServiceCollection services)
        {
            services.AddScoped<ExceptionsHandlerMiddleware>();
            return services;
        }
        
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionsHandlerMiddleware>();
            return builder;
        }
    }
}