using System.Threading.Tasks;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests.Domain
{
    /// <summary>
    /// Test non server errors - postgres = client-side errors, ensure message from postgres is rethrown.
    /// </summary>
    public class ClientError : TestBase
    {
        /// <summary>
        /// Return postgres error
        /// </summary>
        [Test]
        public async Task ReturnErrorIfPostgresError()
        {
            var request = new Request
            {
                Query = "i am not a query",
                Type = QueryType.Oltp,
                IsSelect = true
            };

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();

            SetLocalhostStatisticsAsOnline(storage);

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryFailed);

            Assert.True(result.Message.Contains("Query is not correct"));
        }

        /// <summary>
        /// Enqueue postgres error to response storage
        /// </summary>
        [Test]
        public async Task SetErrorToStorageIfPostgresError()
        {
            var request = new Request
            {
                Query = "i am not a query",
                Type = QueryType.Oltp,
                IsSelect = true,
                AcceptRetries = true
            };

            var storage = ServiceProvider.Resolve<IStatisticsStorage>();

            SetLocalhostStatisticsAsOnline(storage);

            var service = ServiceProvider.Resolve<IQueryDistributionService>();

            var result = await service.DistributeQueryAsync(request);

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryQueued);
            Assert.NotNull(result.RequestId);

            var responseStorage = ServiceProvider.Resolve<IResponseStorage>();

            if (responseStorage.TryGetResponseByRequestId(result.RequestId.Value, out var response))
            {
                Assert.True(response.Message.Contains("Query is not correct"));
            }
            else
                Assert.Fail();
        }
    }
}