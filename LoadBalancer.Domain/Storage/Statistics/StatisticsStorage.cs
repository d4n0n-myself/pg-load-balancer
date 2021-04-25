using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Domain.Extensions;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Domain.Storage.Statistics
{
    /// <inheritdoc />
    public class StatisticsStorage : IStatisticsStorage
    {
        private ConcurrentDictionary<Server, Models.Entities.Statistics> _olapStatisticsMap;
        private ConcurrentDictionary<Server, Models.Entities.Statistics> _oltpStatisticsMap;

        /// <summary>
        /// ctor.
        /// </summary>
        public StatisticsStorage(IOptions<BalancerConfiguration> options)
        {
            var configuration = options.Value;

            _olapStatisticsMap = new ConcurrentDictionary<Server, Models.Entities.Statistics>(
                configuration.OlapPool.MapConfigurationSection()
            );
            _oltpStatisticsMap = new ConcurrentDictionary<Server, Models.Entities.Statistics>(
                configuration.OltpPool.MapConfigurationSection()
            );
        }

        /// <inheritdoc />
        public IDictionary<Server, Models.Entities.Statistics> Get(QueryType type)
        {
            return type switch
            {
                QueryType.Olap => _olapStatisticsMap.ToDictionary(x => x.Key, x => x.Value),
                QueryType.Oltp => _oltpStatisticsMap.ToDictionary(x => x.Key, x => x.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown query type to get stats to!")
            };
        }

        /// <inheritdoc />
        public IEnumerable<(Server, Models.Entities.Statistics)> GetAll()
        {
            return _olapStatisticsMap
                .Concat(_oltpStatisticsMap)
                .Select(x =>
                {
                    var (key, value) = x;
                    return (key, value);
                });
        }

        /// <inheritdoc />
        public void Set(QueryType type, Server server, Models.Entities.Statistics statistics)
        {
            _ = type == QueryType.Oltp
                ? _oltpStatisticsMap.AddOrUpdate(server, statistics, (_, _) => statistics)
                : _olapStatisticsMap.AddOrUpdate(server, statistics, (_, _) => statistics);
        }

        /// <inheritdoc />
        public void ReloadFromConfiguration()
        {
            var options =
                (IOptions<BalancerConfiguration>) ApplicationContext.Container.GetService(
                    typeof(IOptions<BalancerConfiguration>));
            if (options == null)
                throw new ApplicationException("No options to reload configuration in statistics storage!");
            var configuration = options.Value;

            _olapStatisticsMap = new ConcurrentDictionary<Server, Models.Entities.Statistics>(
                configuration.OlapPool.MapConfigurationSection()
            );
            _oltpStatisticsMap = new ConcurrentDictionary<Server, Models.Entities.Statistics>(
                configuration.OltpPool.MapConfigurationSection()
            );
        }
    }
}