using System;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    internal class MatchSettings
    {
        public Guid MatchId { get; set; }
        public string Map { get; set; }
        public string Whitelist { get; set; }
    }
}