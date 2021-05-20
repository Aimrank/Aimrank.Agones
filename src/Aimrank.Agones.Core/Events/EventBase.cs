using System;

namespace Aimrank.Agones.Core.Events
{
    public abstract class EventBase : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}