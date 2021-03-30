using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public partial class Mapping
    {
        [Test]
        public void MapStatistics()
        {
            var expected = new Statistics {IsOnline = true, CurrentSessionsCount = 4};
            var actual = ReadFromFile<Statistics>();
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}