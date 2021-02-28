using System.Collections.Generic;
using LoadBalancer.Models;

namespace LoadBalancer.Domain
{
    public interface IStatisticsStorage
    {
        Statistics Get(Server server);
        void Set(Server server, Statistics statistics);
        IEnumerable<(Server, Statistics)> GetAll();
    }
}