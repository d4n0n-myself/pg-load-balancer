using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LoadBalancer.Models;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Domain
{
    public class StatisticsStorage : IStatisticsStorage
    {
        private readonly ConcurrentDictionary<Server, Statistics> _values;

        public StatisticsStorage(IOptions<BalancerConfiguration> options)
        {
            var configuration = options.Value;

            var mapped =
                configuration.OlapPool
                    .Concat(configuration.OltpPool)
                    .ToDictionary(x => x, x => Statistics.Empty);

            _values = new ConcurrentDictionary<Server, Statistics>(mapped);
        }

        public Statistics Get(Server server)
        {
            if (_values.TryGetValue(server, out var statistics))
            {
                return statistics;
            }

            throw new Exception($"Failed to read statistics for server {server.Host}"); // todo
        }

        public IEnumerable<(Server, Statistics)> GetAll()
        {
            return _values.Select(x =>
            {
                var (key, value) = x;
                return (key, value);
            });
        }

        public void Set(Server server, Statistics statistics)
        {
            _values.AddOrUpdate(server, statistics, (_, _) => statistics);
        }
    }
}