using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Check statistics mappings.
    /// </summary>
    public partial class Mapping
    {
        /// <summary>
        /// Map <see cref="Statistics"/> from file.
        /// </summary>
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