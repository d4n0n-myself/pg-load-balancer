using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Validate responses.
    /// </summary>
    public partial class Validation
    {
        [Test]
        public void ValidateResponse()
        {
            var response = new Response();

            Assert.False(response.Validate(out _));

            response = new Response
            {
                Result = QueryExecutionResult.QueryFailed
            };

            Assert.False(response.Validate(out _));


            response = new Response
            {
                Result = QueryExecutionResult.QueryCompleted
            };

            Assert.True(response.Validate(out _));

            response = new Response
            {
                Result = QueryExecutionResult.QueryFailed,
                Message = "Error message"
            };

            Assert.True(response.Validate(out _));
        }
    }
}