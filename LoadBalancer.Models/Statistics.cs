namespace LoadBalancer.Models
{
    public class Statistics
    {
        public bool IsOnline { get; init; }
        public int CurrentSessionsCount { get; init; }

        public static Statistics Empty => new();
    }
}