using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public partial class Mapping
    {
        [Test]
        public void MapServer()
        {
            var expected = new Server
            {
                Host = "1.2.3.4",
                Port = "1234",
                Database = "test_db",
                Username = "d4n0n_myself",
                Password = "qwerty123",
                Name = "my test server"
            };
            var actual = ReadFromFile<Server>();
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}