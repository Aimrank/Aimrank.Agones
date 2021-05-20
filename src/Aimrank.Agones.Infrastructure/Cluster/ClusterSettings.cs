namespace Aimrank.Agones.Infrastructure.Cluster
{
    internal class ClusterSettings
    {
        public string HostName { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelay { get; set; }
    }
}