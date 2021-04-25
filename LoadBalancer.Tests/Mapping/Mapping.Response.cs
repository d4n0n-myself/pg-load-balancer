using System;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Check response mappings.
    /// </summary>
    public partial class Mapping
    {
        /// <summary>
        /// Map <see cref="Response"/> from file.
        /// </summary>
        [Test]
        public void MapResponse()
        {
            var expected = new Response
            {
                Data = JsonConvert.SerializeObject(new[] {new Statistics {IsOnline = true, CurrentSessionsCount = 2}}),
                Message = "test",
                Result = QueryExecutionResult.QueryCompleted,
                RequestId = Guid.Parse("b2828949-6d7e-4d27-9f5e-b966fd829784"),
            };
            var actual = ReadFromFile<Response>();
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}