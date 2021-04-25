using System.Threading.Tasks;
using LoadBalancer.Domain.Decision;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.System;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests.Domain
{
    /// <summary>
    /// Test server decision functions.
    /// </summary>
    public class ServerDecider : TestBase
    {
        /// <summary>
        /// Choose online server.
        /// </summary>
        [Test]
        public async Task ChooseOnlineServer()
        {
            var request = new Request
            {
                Query = "SELECT 1",
                Type = QueryType.Oltp,
                IsSelect = true
            };

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();

            SetLocalhostStatisticsAsOnline(storage);

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryCompleted);
        }
        
        /// <summary>
        /// Choose free server of two online servers.
        /// </summary>
        [Test]
        public void ChooseEmptyServer()
        {
            var configuration = ServiceProvider.Configuration<BalancerConfiguration>();
            var maxSessions = configuration.GetMaxSessionsParameter(QueryType.Oltp);
            var statisticsStorage = ServiceProvider.Resolve<IStatisticsStorage>();
            var someOccupiedHost = new Server { Host = "192.168.0.3" } ;
            var someEmptyHost = new Server { Host = "192.168.0.4" } ;
            var maxSessionsStats = new Statistics {IsOnline = true, CurrentSessionsCount = 5};
            
            SetLocalhostStatisticsAsOnline(statisticsStorage);
            
            statisticsStorage.Set(QueryType.Oltp, someOccupiedHost, maxSessionsStats);
            statisticsStorage.Set(QueryType.Oltp, someEmptyHost, new Statistics { IsOnline = true });
            var oltpServers = statisticsStorage.Get(QueryType.Oltp);
            
            var decider = ServiceProvider.Resolve<IServerDecider>();
            var availableServer = decider.FindAvailableServer(oltpServers, maxSessions);
            
            Assert.AreNotEqual(availableServer, someOccupiedHost);

            statisticsStorage.ReloadFromConfiguration();
        }
    }
}