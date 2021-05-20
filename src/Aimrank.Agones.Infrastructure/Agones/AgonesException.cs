using System;

namespace Aimrank.Agones.Infrastructure.Agones
{
    internal class AgonesException : Exception
    {
        public AgonesException(string message) : base(message)
        {
        }
    }
}