using System;
using System.Threading.Tasks;
using Dapper;
using LoadBalancer.Models.Entities;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace LoadBalancer.Database.Statistics
{
    public class StatisticsRepository : IStatisticsRepository, IDisposable
    {
        private readonly ILogger<StatisticsRepository> _logger;

        public StatisticsRepository(ILogger<StatisticsRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Models.Entities.Statistics> GetStatistics(Server server)
        {
            await using var npgsqlConnection = new NpgsqlConnection(server.AsConnectionString());
            try
            {
                var sessionsCount =
                    await npgsqlConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM pg_stat_activity");
                var successfulStatistics = new Models.Entities.Statistics
                {
                    CurrentSessionsCount = sessionsCount,
                    IsOnline = true
                };
                return successfulStatistics;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Failed to retrieve statistics for server {server.Host}: {e.Message}");
                var failedStatistics = new Models.Entities.Statistics
                {
                    IsOnline = false
                };
                return failedStatistics;
            }
        }
        
        public void Dispose()
        {
            // do nothing
        }
    }
}