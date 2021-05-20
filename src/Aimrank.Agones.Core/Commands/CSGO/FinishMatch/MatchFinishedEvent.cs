using Aimrank.Agones.Core.Events;
using System;

namespace Aimrank.Agones.Core.Commands.CSGO.FinishMatch
{
    public class MatchFinishedEvent : EventBase
    {
        public Guid MatchId { get; }
        public int Winner { get; }
        public FinishMatchCommand.MatchEndEventTeam TeamTerrorists { get; }
        public FinishMatchCommand.MatchEndEventTeam TeamCounterTerrorists { get; }

        public MatchFinishedEvent(
            Guid matchId,
            int winner, 
            FinishMatchCommand.MatchEndEventTeam teamTerrorists,
            FinishMatchCommand.MatchEndEventTeam teamCounterTerrorists)
        {
            MatchId = matchId;
            Winner = winner;
            TeamTerrorists = teamTerrorists;
            TeamCounterTerrorists = teamCounterTerrorists;
        }
    }
}