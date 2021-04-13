using System.Threading.Tasks;
using LoadBalancer.Database.Statistics;
using LoadBalancer.Models.Entities;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;

namespace LoadBalancer.Tests.Database
{
    /// <summary>
    /// Test statistics storage functionality.
    /// </summary>
    public class Statistics
    {
        private readonly StatisticsRepository _repository =
            new(new Logger<StatisticsRepository>(new NLogLoggerFactory()));

        /// <summary>
        /// Return online == true if server is available 
        /// </summary>
        [Test]
        public async Task IsOnline_IsTrue_IfServerAvailable()
        {
            var goodServer = new Server();
            var statistics = await _repository.GetStatistics(goodServer);
            Assert.True(statistics.IsOnline);
        }

        /// <summary>
        /// Return online == false if server is not available 
        /// </summary>
        /// <remarks>Npgsql timeout = 15sec.</remarks>
        [Test]
        public async Task IsOnline_IsFalse_IfServerNotAvailable()
        {
            var badServer = new Server
            {
                Database = "bad_db", Host = "192.168.1.254", Port = "65534", Username = "pg", Password = "pg"
            };
            var statistics = await _repository.GetStatistics(badServer);
            Assert.False(statistics.IsOnline);
        }
    }
}