using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http;
using System;

namespace Aimrank.Agones.Infrastructure.Cluster
{
    internal static class Extensions
    {
        public static IServiceCollection AddCluster(this IServiceCollection services, IConfiguration configuration)
        {
            var clusterSettings = configuration.GetSection(nameof(ClusterSettings)).Get<ClusterSettings>();
            
            services.AddHttpClient<IClusterService, ClusterService>(c =>
            {
                c.BaseAddress = new Uri($"http://{clusterSettings.HostName}/api/pod/");
            })
                .AddPolicyHandler(GetRetryPolicy(clusterSettings));
                
            return services;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ClusterSettings settings) => Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => r.IsSuccessStatusCode is false)
            .WaitAndRetryAsync(settings.RetryCount, _ => TimeSpan.FromSeconds(settings.RetryDelay));
    }
}