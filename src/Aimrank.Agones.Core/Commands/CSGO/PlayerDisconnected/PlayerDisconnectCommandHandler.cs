using Aimrank.Agones.Core.Events;
using Aimrank.Agones.Core.Services;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Aimrank.Agones.Core.Commands.CSGO.PlayerDisconnected
{
    internal class PlayerDisconnectCommandHandler : IRequestHandler<PlayerDisconnectCommand>
    {
        private readonly IEventsDispatcher _dispatcher;
        private readonly IServerProcessManager _serverProcessManager;

        public PlayerDisconnectCommandHandler(IEventsDispatcher dispatcher, IServerProcessManager serverProcessManager)
        {
            _dispatcher = dispatcher;
            _serverProcessManager = serverProcessManager;
        }

        public Task<Unit> Handle(PlayerDisconnectCommand request, CancellationToken cancellationToken)
        {
            var matchId = _serverProcessManager.GetMatchId();
            if (matchId.HasValue)
            {
                _dispatcher.Dispatch(new PlayerDisconnectedEvent(matchId.Value, request.SteamId));
            }
            
            return Task.FromResult(Unit.Value);
        }
    }
}