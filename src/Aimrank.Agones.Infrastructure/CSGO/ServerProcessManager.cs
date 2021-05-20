using Aimrank.Agones.Core.Services;
using Aimrank.Agones.Infrastructure.Agones;
using Aimrank.Agones.Infrastructure.Cluster;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    internal sealed class ServerProcessManager : IServerProcessManager, IDisposable
    {
        private readonly IAgonesService _agonesService;
        private readonly IClusterService _clusterService;
        private readonly ILogger<ServerProcessManager> _logger;

        private ServerProcess _serverProcess;
        private MatchSettings _matchSettings;

        public ServerProcessManager(
            IAgonesService agonesService,
            IClusterService clusterService,
            ILogger<ServerProcessManager> logger)
        {
            _agonesService = agonesService;
            _clusterService = clusterService;
            _logger = logger;
        }

        public async Task StartServerAsync()
        {
            await _agonesService.ConnectAsync();
            var steamToken = await _clusterService.ConnectAsync(_agonesService.GetAddress());
            if (steamToken is null)
            {
                await _agonesService.ShutDownAsync();
                return;
            }
            
            _serverProcess = new ServerProcess(steamToken);
            _serverProcess.Start();
            
            _logger.LogInformation($"Started CS:GO server");
        }

        public Guid? GetMatchId() => _matchSettings?.MatchId;

        public async Task StartMatchAsync(Guid matchId, string map, string whitelist)
        {
            _matchSettings = new MatchSettings
            {
                MatchId = matchId,
                Map = map,
                Whitelist = whitelist
            };

            await _serverProcess.StartMatchAsync(_matchSettings);
        }

        public async Task ServerStartedAsync()
        {
            await _agonesService.ReadyAsync();
        }

        public async Task ServerStoppedAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(20));
            await ShutDownAsync();
        }
        
        public void Dispose() => _serverProcess?.Dispose();
        
        private async Task ShutDownAsync()
        {
            await _agonesService.ShutDownAsync();
        }

        public async Task DisconnectAsync()
        {
            await _clusterService.DisconnectAsync(_agonesService.GetAddress());
        }
    }
}