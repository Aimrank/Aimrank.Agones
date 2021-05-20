using Aimrank.Agones.Core.Events;
using System;

namespace Aimrank.Agones.Core.Commands.CSGO.CancelMatch
{
    public class MatchCanceledEvent : EventBase
    {
        public Guid MatchId { get; }

        public MatchCanceledEvent(Guid matchId)
        {
            MatchId = matchId;
        }
    }
}