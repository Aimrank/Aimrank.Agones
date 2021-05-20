using System.Threading.Tasks;

namespace Aimrank.Agones.Infrastructure.Cluster
{
    internal interface IClusterService
    {
        Task<string> ConnectAsync(string address);
        Task DisconnectAsync(string address);
    }
}