using MediatR;
using System.Collections.Generic;

namespace Aimrank.Agones.Core.Commands.CSGO.FinishMatch
{
    public class FinishMatchCommand : IRequest
    {
        public int Winner { get; }
        public MatchEndEventTeam TeamTerrorists { get; }
        public MatchEndEventTeam TeamCounterTerrorists { get; }

        public FinishMatchCommand(
            int winner,
            MatchEndEventTeam teamTerrorists,
            MatchEndEventTeam teamCounterTerrorists)
        {
            Winner = winner;
            TeamTerrorists = teamTerrorists;
            TeamCounterTerrorists = teamCounterTerrorists;
        }

        public record MatchEndEventTeam(int Score, IEnumerable<MatchEndEventPlayer> Clients);
        public record MatchEndEventPlayer(string SteamId, string Name, int Kills, int Assists, int Deaths, int Hs);
    }
}