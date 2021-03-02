using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Extensions
{
    internal static class ServerExtensions
    {
        internal static Dictionary<Server, Statistics> MapConfigurationSection(this IEnumerable<Server> servers)
        {
            return servers.ToDictionary(x => x, _ => Statistics.Empty);
        }
    }
}