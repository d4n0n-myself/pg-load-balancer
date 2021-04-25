using System.Collections.Generic;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Domain.Storage.Statistics
{
    /// <summary>
    /// A <see cref="Statistics"/> storage.
    /// </summary>
    public interface IStatisticsStorage
    {
        /// <summary>
        /// Get all server statistics for query type.
        /// </summary>
        IDictionary<Server, Models.Entities.Statistics> Get(QueryType type);

        /// <summary>
        /// Set statistics for server.
        /// </summary>
        void Set(QueryType type, Server server, Models.Entities.Statistics statistics);

        /// <summary>
        /// Get all statistics.
        /// </summary>
        IEnumerable<(Server, Models.Entities.Statistics)> GetAll();

        /// <summary>
        /// Reload from read configuration.
        /// </summary>
        void ReloadFromConfiguration();
    }
}