namespace LoadBalancer.Models
{
    public class BalancerConfiguration
    {
        public long OlapMaxSessions { get; init; }
        public long OltpMaxSessions { get; init; }

        public int RefreshOlapStatisticsIntervalInSec { get; init; }
        public int RefreshOltpStatisticsIntervalInSec { get; init; }

        public Server[] OlapPool { get; init; }
        public Server[] OltpPool { get; init; }
    }
}