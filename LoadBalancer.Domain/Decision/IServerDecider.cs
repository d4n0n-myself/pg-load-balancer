using System.Collections.Generic;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Decision
{
    /// <summary>
    /// Interface to handle server decision rules.
    /// </summary>
    public interface IServerDecider
    {
        /// <summary>
        /// Get available server metadata by announced rules.
        /// </summary>
        Server FindAvailableServer(IDictionary<Server, Statistics> servers, long maxSessions);
    }
}