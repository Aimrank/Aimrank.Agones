namespace Aimrank.Agones.Infrastructure.Agones
{
    internal static class AgonesGameServerStatus
    {
        public const string Scheduled = nameof(Scheduled);
        public const string Reserved = nameof(Reserved);
        public const string RequestReady = nameof(RequestReady);
        public const string Ready = nameof(Ready);
        public const string Allocated = nameof(Allocated);
        public const string Unhealthy = nameof(Unhealthy);
        public const string Shutdown = nameof(Shutdown);
    }
}