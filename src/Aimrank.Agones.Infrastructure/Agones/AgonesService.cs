using Agones.Dev.Sdk;
using Agones;
using Aimrank.Agones.Core.Commands.StartMatch;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Aimrank.Agones.Infrastructure.Agones
{
    internal sealed class AgonesService : IAgonesService
    {
        private const string AnnotationMap = "agones.dev/sdk-map";
        private const string AnnotationMatchId = "agones.dev/sdk-matchId";
        private const string AnnotationWhitelist = "agones.dev/sdk-whitelist";
        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AgonesService> _logger;
        private readonly AgonesSDK _agones = new();
        private GameServer _gameServer;

        public AgonesService(IServiceProvider serviceProvider, ILogger<AgonesService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            
            _agones.WatchGameServer(async server =>
            {
                switch (server.Status.State)
                {
                    case AgonesGameServerStatus.Allocated:
                        await ServerAllocatedAsync(server);
                        break;
                }
            });
        }

        public string GetAddress()
        {
            if (_gameServer is null)
            {
                return null;
            }

            var port = _gameServer.Status.Ports.FirstOrDefault(p => p.Name.StartsWith("srcds"));
            return $"{_gameServer.Status.Address}:{port.Port_}";
        }

        public async Task ConnectAsync()
        {
            if (!await _agones.ConnectAsync())
            {
                throw new AgonesException("Could not connect to agones.");
            }
            
            _gameServer = await _agones.GetGameServerAsync();
            
            _logger.LogInformation("Connection with agones sidecar established.");
        }
        
        public async Task ReadyAsync()
        {
            if (_gameServer.Status.State != AgonesGameServerStatus.Scheduled)
            {
                return;
            }

            await _agones.ReadyAsync();
        }

        public Task ShutDownAsync() => _agones.ShutDownAsync();

        private async Task ServerAllocatedAsync(GameServer server)
        {
            if (_gameServer is not null && _gameServer.Status.State == server.Status.State)
            {
                return;
            }

            _gameServer = server;
            
            var command = new StartMatchCommand(
                Guid.Parse(server.ObjectMeta.Annotations[AnnotationMatchId]),
                server.ObjectMeta.Annotations[AnnotationMap],
                server.ObjectMeta.Annotations[AnnotationWhitelist]);

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}