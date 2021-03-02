// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Models.System
{
    public class BalancerConfiguration
    {
        public long MaxRetryCount { get; init; }

        public long OlapMaxSessions { get; init; }
        public long OltpMaxSessions { get; init; }

        public int RefreshOlapStatisticsIntervalInSec { get; init; }
        public int RefreshOltpStatisticsIntervalInSec { get; init; }

        public Server[] OlapPool { get; init; }
        public Server[] OltpPool { get; init; }

        public long GetMaxSessionsParameter(QueryType type)
        {
            return type == QueryType.Olap ? OlapMaxSessions : OltpMaxSessions;
        }
    }
}