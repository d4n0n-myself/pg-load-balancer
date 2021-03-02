using System.Collections.Generic;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Domain.Storage
{
    public interface IStatisticsStorage
    {
        IDictionary<Server, Statistics> Get(QueryType type);
        void Set(QueryType type, Server server, Statistics statistics);
        IEnumerable<(Server, Statistics)> GetAll();
    }
}