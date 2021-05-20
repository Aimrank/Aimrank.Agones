using Aimrank.Agones.Core.Events;
using System;

namespace Aimrank.Agones.Core.Commands.CSGO.PlayerDisconnected
{
    public class PlayerDisconnectedEvent : EventBase
    {
        public Guid MatchId { get; }
        public string SteamId { get; }

        public PlayerDisconnectedEvent(Guid matchId, string steamId)
        {
            MatchId = matchId;
            SteamId = steamId;
        }
    }
}