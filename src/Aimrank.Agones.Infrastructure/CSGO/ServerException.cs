using System;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
        }
    }
}