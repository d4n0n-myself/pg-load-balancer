using System.Linq;
using System.Threading.Tasks;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests.Domain
{
    /// <summary>
    /// Test queues functionality.
    /// </summary>
    public class Queue : TestBase
    {
        /// <summary>
        /// Enqueue success.
        /// </summary>
        [Test]
        public async Task ReturnIsSuccess()
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
        
        /// <summary>
        /// First request to be handled from queue is with max priority possible.
        /// </summary>
        [Test]
        public void PriorityWorks()
        {
            var request1 = new Request {Priority = 1};
            var request2 = new Request {Priority = 2};
            var request3 = new Request {Priority = 3};
            var request4 = new Request {Priority = 4};

            var queue = ServiceProvider.Resolve<IRequestQueue>();

            var requests = new[] {request1, request2, request3, request4};
            foreach (var request in requests)
            {
                queue.Add(request);
            }

            var pop = queue.Get();

            Assert.AreEqual(pop.Priority, requests.Max(x => x.Priority));

            queue.Purge();
        }

        /// <summary>
        /// Attempts mechanism is working.
        /// </summary>
        [Test]
        public async Task CheckAttemptsMechanism()
        {
            var request = new Request
            {
                Query = "SELECT 1 as col",
                Type = QueryType.Oltp,
                IsSelect = true,
                AcceptRetries = true
            };

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);
            
            Assert.AreEqual(result.Result, QueryExecutionResult.QueryQueued);
            Assert.NotNull(result.RequestId);

            var queue = ServiceProvider.Resolve<IRequestQueue>();

            var pop = queue.Get();

            Assert.AreEqual(request, pop);
            
            queue.Add(pop);

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();
            
            SetLocalhostStatisticsAsOnline(storage);
            
            var pop2 = queue.Get();
            Assert.NotNull(pop2);

            var result2 = await service.DistributeQueryAsync(request);
            
            Assert.AreEqual(result2.Result, QueryExecutionResult.QueryCompleted);
            Assert.NotNull(result2.RequestId);

            var responseStorage = ServiceProvider.Resolve<IResponseStorage>();
            
            if (!responseStorage.TryGetResponseByRequestId(result2.RequestId.Value, out var response))
            {
                Assert.Fail("No response!");
            }

            Assert.AreEqual(response.RequestId, result2.RequestId.Value);
            Assert.AreEqual(response.Result, QueryExecutionResult.QueryCompleted);
            Assert.AreEqual(response.Data, "[{\"col\":1}]");
        }
    }
}