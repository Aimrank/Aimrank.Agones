using MediatR;
using System;

namespace Aimrank.Agones.Core.Commands.StartMatch
{
    public class StartMatchCommand : IRequest
    {
        public Guid MatchId { get; }
        public string Map { get; }
        public string Whitelist { get; }

        public StartMatchCommand(Guid matchId, string map, string whitelist)
        {
            MatchId = matchId;
            Map = map;
            Whitelist = whitelist;
        }
    }
}