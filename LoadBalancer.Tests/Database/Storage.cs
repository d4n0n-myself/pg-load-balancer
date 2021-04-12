using System;
using System.Linq;
using System.Text.Json;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Tests.Extensions;
using NUnit.Framework;
using StatisticsM = LoadBalancer.Models.Entities.Statistics;
using ResponseM = LoadBalancer.Models.Entities.Response;
using RequestM = LoadBalancer.Models.Entities.Request;

namespace LoadBalancer.Tests.Database
{
    public class Storage : TestBase
    {
        [Test]
        public void Request()
        {
            var queue = new RequestQueue();
            var expected = new RequestM
            {
                Type = QueryType.Olap,
                IsSelect = true,
                Query = "SELECT * FROM my_table",
                AcceptRetries = true,
                Priority = 1,
                RequestId = Guid.Parse("b2828949-6d7e-4d27-9f5e-b966fd829784"),
                IsRetried = true,
                CurrentRetryAttempt = 2
            };

            queue.Add(expected);
            var actual = queue.Get();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Response()
        {
            var responseStorage = new ResponseStorage();

            var data = JsonSerializer.Serialize(new[] {new StatisticsM {IsOnline = true, CurrentSessionsCount = 5}});
            var guid = Guid.NewGuid();

            var expected = ResponseM.Completed(data, guid);

            responseStorage.Add(expected);

            responseStorage.TryGetResponseByRequestId(guid, out var actual);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Statistics()
        {
            var statisticsStorage = ServiceProvider.Resolve<IStatisticsStorage>();
            var localhost = new Server();
            var stats = new StatisticsM {IsOnline = true, CurrentSessionsCount = 5};
            statisticsStorage.Set(QueryType.Oltp, localhost, stats);
            var oltpServers = statisticsStorage.Get(QueryType.Oltp);

            Assert.AreEqual(oltpServers.Count, 2);
            Assert.True(oltpServers.Any(x => x.Key == localhost));

            var olapServers = statisticsStorage.Get(QueryType.Olap);
            Assert.AreEqual(olapServers.Count, 1);

            var all = statisticsStorage.GetAll().ToArray();
            Assert.AreEqual(olapServers.Count + oltpServers.Count, all.Length);
            Assert.AreEqual(olapServers.Select(x => x.Key).Concat(oltpServers.Select(x => x.Key)).ToArray(),
                all.Select(x => x.Item1).ToArray());
        }
    }
}