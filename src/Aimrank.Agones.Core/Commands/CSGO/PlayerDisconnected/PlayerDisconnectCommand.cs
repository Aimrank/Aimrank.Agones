using MediatR;

namespace Aimrank.Agones.Core.Commands.CSGO.PlayerDisconnected
{
    public class PlayerDisconnectCommand : IRequest
    {
        public string SteamId { get; }

        public PlayerDisconnectCommand(string steamId)
        {
            SteamId = steamId;
        }
    }
}