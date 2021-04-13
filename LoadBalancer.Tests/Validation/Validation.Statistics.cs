using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Validate statistics.
    /// </summary>
    public partial class Validation
    {
        [Test]
        public void ValidateStatistics()
        {
            var statistics = new Statistics
            {
                CurrentSessionsCount = -1
            };

            Assert.False(statistics.Validate(out _));

            statistics = new Statistics();

            Assert.True(statistics.Validate(out _));

            statistics = new Statistics
            {
                IsOnline = true,
                CurrentSessionsCount = 1
            };

            Assert.True(statistics.Validate(out _));
        }
    }
}