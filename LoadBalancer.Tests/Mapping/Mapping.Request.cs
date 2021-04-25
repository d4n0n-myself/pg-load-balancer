using System;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Check request mappings.
    /// </summary>
    public partial class Mapping
    {
        /// <summary>
        /// Map <see cref="Request"/> from file.
        /// </summary>
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