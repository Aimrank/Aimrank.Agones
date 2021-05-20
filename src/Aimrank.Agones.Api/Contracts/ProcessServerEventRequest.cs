namespace Aimrank.Agones.Api.Contracts
{
    public class ProcessServerEventRequest
    {
        public string Name { get; set; }
        public dynamic Data { get; set; }
    }
}