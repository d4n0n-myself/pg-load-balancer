using System.Threading.Tasks;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests.Domain
{
    /// <summary>
    /// Test straight query execution, no queues.
    /// </summary>
    public class Straight : TestBase
    {
        /// <summary>
        /// Return if query is a success.
        /// </summary>
        [Test]
        public async Task ReturnIsSuccess()
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
    }
}