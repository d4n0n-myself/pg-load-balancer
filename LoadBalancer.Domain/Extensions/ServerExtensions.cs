using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Extensions
{
    /// <summary>
    /// Server configuration object extensions.
    /// </summary>
    internal static class ServerExtensions
    {
        /// <summary>
        /// Create empty statistics collection.
        /// </summary>
        internal static Dictionary<Server, Statistics> MapConfigurationSection(this IEnumerable<Server> servers)
        {
            return servers.ToDictionary(x => x, _ => Statistics.Empty);
        }
    }
}