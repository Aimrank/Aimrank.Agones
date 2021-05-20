using MediatR;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    public interface IServerEventMapper
    {
        IRequest Map(string name, dynamic data);
    }
}