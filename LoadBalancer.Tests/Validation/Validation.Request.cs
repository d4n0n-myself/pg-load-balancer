using System;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public partial class Validation
    {
        [Test]
        public void ValidateRequest()
        {
            var request = new Request();

            Assert.False(request.Validate(out _));

            request = new Request
            {
                Type = QueryType.Olap
            };

            Assert.False(request.Validate(out _));

            request = new Request
            {
                Query = "SELECT * FROM table"
            };

            Assert.False(request.Validate(out _));

            request = new Request
            {
                CurrentRetryAttempt = -1
            };

            Assert.False(request.Validate(out _));

            request = new Request
            {
                CurrentRetryAttempt = -1,
                Query = "SELECT * FROM table",
                Type = QueryType.Olap
            };

            Assert.False(request.Validate(out _));

            request = new Request
            {
                Query = "SELECT * FROM table",
                Type = QueryType.Olap
            };

            Assert.True(request.Validate(out _));

            request = new Request
            {
                IsRetried = true,
                Query = "SELECT * FROM table",
                Type = QueryType.Olap
            };

            Assert.False(request.Validate(out _));

            request = new Request
            {
                IsRetried = true,
                RequestId = Guid.NewGuid(),
                Query = "SELECT * FROM table",
                Type = QueryType.Olap
            };

            Assert.True(request.Validate(out _));
        }
    }
}