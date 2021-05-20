using Aimrank.Agones.Core.Events;
using Aimrank.Agones.Core.Services;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Agones.Core.Commands.CSGO.FinishMatch
{
    internal class FinishMatchCommandHandler : IRequestHandler<FinishMatchCommand>
    {
        private readonly IEventsDispatcher _dispatcher;
        private readonly IServerProcessManager _serverProcessManager;

        public FinishMatchCommandHandler(
            IEventsDispatcher dispatcher,
            IServerProcessManager serverProcessManager)
        {
            _dispatcher = dispatcher;
            _serverProcessManager = serverProcessManager;
        }

        public async Task<Unit> Handle(FinishMatchCommand request, CancellationToken cancellationToken)
        {
            var matchId = _serverProcessManager.GetMatchId();
            if (matchId.HasValue)
            {
                _dispatcher.Dispatch(new MatchFinishedEvent(
                    matchId.Value,
                    request.Winner,
                    request.TeamTerrorists,
                    request.TeamCounterTerrorists));
            }

            await _serverProcessManager.ServerStoppedAsync();
            
            return Unit.Value;
        }
    }
}