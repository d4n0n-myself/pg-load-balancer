using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Domain.Extensions;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Domain.Storage
{
    public class StatisticsStorage : IStatisticsStorage
    {
        private readonly ConcurrentDictionary<Server, Statistics> _olapStatisticsMap;
        private readonly ConcurrentDictionary<Server, Statistics> _oltpStatisticsMap;

        public StatisticsStorage(IOptions<BalancerConfiguration> options)
        {
            var configuration = options.Value;

            _olapStatisticsMap = new ConcurrentDictionary<Server, Statistics>(
                configuration.OlapPool.MapConfigurationSection()
            );
            _oltpStatisticsMap = new ConcurrentDictionary<Server, Statistics>(
                configuration.OltpPool.MapConfigurationSection()
            );
        }

        public IDictionary<Server, Statistics> Get(QueryType type)
        {
            return type switch
            {
                QueryType.Olap => _olapStatisticsMap.ToDictionary(x => x.Key, x => x.Value),
                QueryType.Oltp => _oltpStatisticsMap.ToDictionary(x => x.Key, x => x.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown query type to get stats to!")
            };
        }

        public IEnumerable<(Server, Statistics)> GetAll()
        {
            return _olapStatisticsMap
                .Concat(_oltpStatisticsMap)
                .Select(x =>
                {
                    var (key, value) = x;
                    return (key, value);
                });
        }

        public void Set(QueryType type, Server server, Statistics statistics)
        {
            _ = type == QueryType.Oltp
                ? _oltpStatisticsMap.AddOrUpdate(server, statistics, (_, _) => statistics)
                : _olapStatisticsMap.AddOrUpdate(server, statistics, (_, _) => statistics);
        }
    }
}