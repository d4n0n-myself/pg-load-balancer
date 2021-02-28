using System;
using System.Threading.Tasks;
using LoadBalancer.Database;
using LoadBalancer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace LoadBalancer.Domain
{
    public class RetrieveOlapStatisticsTask : IJob
    {
        private readonly BalancerConfiguration _configuration;
        private readonly IStatisticsRepository _repository;
        private readonly IStatisticsStorage _storage;
        private readonly ILogger<RetrieveOlapStatisticsTask> _logger;

        public RetrieveOlapStatisticsTask(IOptions<BalancerConfiguration> configuration,
            IStatisticsRepository repository, IStatisticsStorage storage, ILogger<RetrieveOlapStatisticsTask> logger)
        {
            _repository = repository;
            _storage = storage;
            _logger = logger;
            _configuration = configuration.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Get stats for Olap {DateTime.Now}");
            var servers = _configuration.OlapPool;

            foreach (var server in servers)
            {
                var stats = await _repository.GetStatistics(server);
                _storage.Set(server, stats);
            }
        }
    }
}