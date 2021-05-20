using Aimrank.Agones.Core.Services;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Agones.Core.Commands.StartMatch
{
    internal class StartMatchCommandHandler : IRequestHandler<StartMatchCommand>
    {
        private readonly IServerProcessManager _serverProcessManager;

        public StartMatchCommandHandler(IServerProcessManager serverProcessManager)
        {
            _serverProcessManager = serverProcessManager;
        }

        public async Task<Unit> Handle(StartMatchCommand request, CancellationToken cancellationToken)
        {
            await _serverProcessManager.StartMatchAsync(request.MatchId, request.Map, request.Whitelist);
            
            return Unit.Value;
        }
    }
}