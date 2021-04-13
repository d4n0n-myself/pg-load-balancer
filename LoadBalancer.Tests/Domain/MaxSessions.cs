using System.Threading.Tasks;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests.Domain
{
    /// <summary>
    /// Test MaxSessions parameter handling.
    /// </summary>
    public class MaxSessions : TestBase
    {
        /// <summary>
        /// Deny request if servers are not available.
        /// </summary>
        [Test]
        public async Task Deny()
        {
            var request = new Request
            {
                Query = "SELECT 1",
                Type = QueryType.Oltp,
                IsSelect = true
            };

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();

            SetLocalhostStatisticsAsOnline(storage, 4);

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryFailed);

            Assert.AreEqual(result.Message, "No server available right now.");
        }

        /// <summary>
        /// Enqueue if servers are not available.
        /// </summary>
        [Test]
        public async Task Enqueue()
        {
            var request = new Request
            {
                Query = "SELECT 1",
                Type = QueryType.Oltp,
                IsSelect = true,
                AcceptRetries = true
            };

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();

            SetLocalhostStatisticsAsOnline(storage, 4);

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryQueued);

            var queue = ServiceProvider.Resolve<IRequestQueue>();

            var pop = queue.Get();

            Assert.AreEqual(pop, request);
        }
    }
}