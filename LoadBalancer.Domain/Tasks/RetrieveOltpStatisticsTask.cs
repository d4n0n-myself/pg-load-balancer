using System.Threading.Tasks;
using LoadBalancer.Database.Statistics;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace LoadBalancer.Domain.Tasks
{
    /// <summary>
    /// Oltp server statistics updater job.
    /// </summary>
    public class RetrieveOltpStatisticsTask : IJob
    {
        private readonly BalancerConfiguration _configuration;
        private readonly IStatisticsRepository _repository;
        private readonly IStatisticsStorage _storage;
        private readonly ILogger<RetrieveOltpStatisticsTask> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetrieveOltpStatisticsTask(IOptions<BalancerConfiguration> configuration,
            IStatisticsRepository repository, IStatisticsStorage storage, ILogger<RetrieveOltpStatisticsTask> logger)
        {
            _repository = repository;
            _storage = storage;
            _logger = logger;
            _configuration = configuration.Value;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Get stats for Oltp");
            var servers = _configuration.OltpPool;

            foreach (var server in servers)
            {
                var stats = await _repository.GetStatistics(server);
                _storage.Set(QueryType.Oltp, server, stats);
            }
        }
    }
}