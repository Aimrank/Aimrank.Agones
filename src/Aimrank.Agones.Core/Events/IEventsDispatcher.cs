namespace Aimrank.Agones.Core.Events
{
    public interface IEventsDispatcher
    {
        void Dispatch(IEvent @event);
    }
}