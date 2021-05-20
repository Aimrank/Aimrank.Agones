using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aimrank.Agones.Infrastructure.Cluster
{
    internal sealed class ClusterService : IClusterService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClusterService> _logger;

        public ClusterService(HttpClient httpClient, ILogger<ClusterService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> ConnectAsync(string address)
        {
            var response = await _httpClient.PostAsJsonAsync("register", new {Address = address});

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Could not connect with cluster.");
                return null;
            }
                
            var content = await response.Content.ReadFromJsonAsync<ClusterResponse>();
            return content.SteamToken;
        }

        public async Task DisconnectAsync(string address)
        {
            var response = await _httpClient.PostAsJsonAsync("unregister", new {Address = address});

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Could not safely disconnect from cluster.");
            }
        }
    }
}