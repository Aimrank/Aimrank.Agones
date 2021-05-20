using Aimrank.Agones.Core.Events;
using Aimrank.Agones.Core.Services;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Agones.Core.Commands.CSGO.CancelMatch
{
    internal class CancelMatchCommandHandler : IRequestHandler<CancelMatchCommand>
    {
        private readonly IEventsDispatcher _dispatcher;
        private readonly IServerProcessManager _serverProcessManager;

        public CancelMatchCommandHandler(
            IEventsDispatcher dispatcher,
            IServerProcessManager serverProcessManager)
        {
            _dispatcher = dispatcher;
            _serverProcessManager = serverProcessManager;
        }

        public async Task<Unit> Handle(CancelMatchCommand request, CancellationToken cancellationToken)
        {
            var matchId = _serverProcessManager.GetMatchId();
            if (matchId.HasValue)
            {
                _dispatcher.Dispatch(new MatchCanceledEvent(matchId.Value));
            }

            await _serverProcessManager.ServerStoppedAsync();
            
            return Unit.Value;
        }
    }
}