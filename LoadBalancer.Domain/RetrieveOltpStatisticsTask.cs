using System;
using System.Threading.Tasks;
using LoadBalancer.Database;
using LoadBalancer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace LoadBalancer.Domain
{
    public class RetrieveOltpStatisticsTask : IJob
    {
        private readonly BalancerConfiguration _configuration;
        private readonly IStatisticsRepository _repository;
        private readonly IStatisticsStorage _storage;
        private readonly ILogger<RetrieveOltpStatisticsTask> _logger;

        public RetrieveOltpStatisticsTask(IOptions<BalancerConfiguration> configuration,
            IStatisticsRepository repository, IStatisticsStorage storage, ILogger<RetrieveOltpStatisticsTask> logger)
        {
            _repository = repository;
            _storage = storage;
            _logger = logger;
            _configuration = configuration.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Get stats for Oltp {DateTime.Now}");
            var servers = _configuration.OltpPool;

            foreach (var server in servers)
            {
                var stats = await _repository.GetStatistics(server);
                _storage.Set(server, stats);
            }
        }
    }
}