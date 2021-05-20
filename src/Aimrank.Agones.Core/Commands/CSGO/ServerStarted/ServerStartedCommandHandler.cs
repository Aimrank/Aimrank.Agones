using Aimrank.Agones.Core.Services;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Agones.Core.Commands.CSGO.ServerStarted
{
    internal class ServerStartedCommandHandler : IRequestHandler<ServerStartedCommand>
    {
        private readonly IServerProcessManager _serverProcessManager;

        public ServerStartedCommandHandler(IServerProcessManager serverProcessManager)
        {
            _serverProcessManager = serverProcessManager;
        }

        public async Task<Unit> Handle(ServerStartedCommand request, CancellationToken cancellationToken)
        {
            await _serverProcessManager.ServerStartedAsync();
            
            return Unit.Value;
        }
    }
}