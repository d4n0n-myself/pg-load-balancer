// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Models.System
{
    /// <summary>
    /// Balancing configuration for application. Is being read from appsettings.json file. 
    /// </summary>
    public class BalancerConfiguration
    {
        /// <summary>
        /// Max query execution retry count. 
        /// </summary>
        public long MaxRetryCount { get; init; }
        
        /// <summary>
        /// Max active transactions on OLAP server.
        /// </summary>
        public long OlapMaxSessions { get; init; }
        
        /// <summary>
        /// Max active transactions on OLTP server.
        /// </summary>
        public long OltpMaxSessions { get; init; }

        /// <summary>
        /// Statistics refresh interval for OLAP servers. 
        /// </summary>
        public int RefreshOlapStatisticsIntervalInSec { get; init; }
        
        /// <summary>
        /// Statistics refresh interval for OLTP servers. 
        /// </summary>
        public int RefreshOltpStatisticsIntervalInSec { get; init; }

        /// <summary>
        /// OLAP server configurations.
        /// </summary>
        public Server[] OlapPool { get; init; }
        
        /// <summary>
        /// OLTP server configurations.
        /// </summary>
        public Server[] OltpPool { get; init; }

        /// <summary>
        /// Get max sessions. Convenient in case of extensiosn 
        /// </summary>
        public long GetMaxSessionsParameter(QueryType type)
        {
            return type == QueryType.Olap ? OlapMaxSessions : OltpMaxSessions;
        }
    }
}