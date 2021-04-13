using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Decision
{
    /// <inheritdoc />
    public class ServerDecider : IServerDecider
    {
        /// <inheritdoc />
        public Server FindAvailableServer(IDictionary<Server, Statistics> servers, long maxSessions)
        {
            var (availableServer, _) = servers
                .FirstOrDefault(x => x.Value.IsOnline && x.Value.CurrentSessionsCount < maxSessions);
            return availableServer;
        }
    }
}