using System;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public partial class Mapping
    {
        [Test]
        public void MapRequest()
        {
            var expected = new Request
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

            var actual = ReadFromFile<Request>();
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}