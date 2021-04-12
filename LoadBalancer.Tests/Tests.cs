using System.Threading.Tasks;
using LoadBalancer.Domain.Services;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public class Tests : TestBase
    {
        #region TestSuccess
        
        [Test]
        public async Task ReturnWithoutRetryIsSuccess()
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
        
        [Test]
        public async Task ReturnWithRetryIsSuccess()
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

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryCompleted);

            var queue = ServiceProvider.Resolve<IRequestQueue>();

            var pop = queue.Get();

            Assert.AreEqual(pop, request);
        }

        #endregion

        #region Domain
        [Test]
        public void ChooseOnlineServer()
        {
            // todo implement IServerChooser ? IServerDecider ? wtvr 
        }
        
        [Test]
        public void ChooseAvailableServer()
        {
        }

        #endregion

        #region postgres error

        /// <summary>
        /// Вернуть ошибку postgres
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
        /// Поставить в response storage ошибку postgres
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

        #endregion

        #region MaxSessions parameter

        [Test]
        public async Task DenyIfTooManyConnects()
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

        [Test]
        public async Task EnqueueIfTooManyConnects()
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

            Assert.AreEqual(result.Result, QueryExecutionResult.QueryCompleted);

            var queue = ServiceProvider.Resolve<IRequestQueue>();

            var pop = queue.Get();

            Assert.AreEqual(pop, request);
        }
        
        public void PriorityWorks()
        {
            // todo
        }

        public void CheckAttemptsMechanism()
        {
            // todo
        }

        #endregion
    }
}