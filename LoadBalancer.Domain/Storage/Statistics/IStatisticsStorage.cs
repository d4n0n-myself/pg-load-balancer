using System.Collections.Generic;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Domain.Storage.Statistics
{
    public interface IStatisticsStorage
    {
        IDictionary<Server, Models.Entities.Statistics> Get(QueryType type);
        void Set(QueryType type, Server server, Models.Entities.Statistics statistics);
        IEnumerable<(Server, Models.Entities.Statistics)> GetAll();
    }
}