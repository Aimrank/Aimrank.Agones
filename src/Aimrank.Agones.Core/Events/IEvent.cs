using System;

namespace Aimrank.Agones.Core.Events
{
    public interface IEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
    }
}