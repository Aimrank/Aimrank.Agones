using System.Threading.Tasks;

namespace Aimrank.Agones.Infrastructure.Agones
{
    internal interface IAgonesService
    {
        string GetAddress();
        Task ConnectAsync();
        Task ReadyAsync();
        Task ShutDownAsync();
    }
}