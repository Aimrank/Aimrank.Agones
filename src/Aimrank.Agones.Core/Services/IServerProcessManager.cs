using System.Threading.Tasks;
using System;

namespace Aimrank.Agones.Core.Services
{
    public interface IServerProcessManager
    {
        Guid? GetMatchId();
        Task StartServerAsync();
        Task StartMatchAsync(Guid matchId, string map, string whitelist);
        Task ServerStartedAsync();
        Task ServerStoppedAsync();
        Task DisconnectAsync();
    }
}