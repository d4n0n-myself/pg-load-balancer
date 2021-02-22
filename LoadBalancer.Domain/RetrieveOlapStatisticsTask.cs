using System.Linq;
using System.Threading.Tasks;
using LoadBalancer.Database;
using LoadBalancer.Models;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace LoadBalancer.Domain
{
    public class RetrieveOlapStatisticsTask : IJob
    {
        private readonly IConfiguration _configuration;

        public RetrieveOlapStatisticsTask(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            var servers = _configuration.GetSection("OlapPool")
                .Get<string[]>()
                .Select(Server.FromConnectionString);

            foreach (var server in servers)
            {
                var stats = await new StatisticsRepository().GetStatistics(server);
                // set stats
            }
        }
    }
}