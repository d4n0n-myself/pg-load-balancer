using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public partial class Validation
    {
        [Test]
        public void ValidateServer()
        {
            var server = new Server();

            Assert.False(server.Validate(out _));

            server = new Server()
            {
                Host = "123",
                Port = "1234",
                Database = "asd",
                Username = "asd",
                Password = "asd",
            };

            Assert.False(server.Validate(out _));

            server = new Server()
            {
                Port = "asdas",
                Database = "asd",
                Host = "1.2.3.4",
                Username = "asd",
                Password = "asd",
            };

            Assert.False(server.Validate(out _));

            server = new Server()
            {
                Database = "asd",
                Host = "1.2.3.4",
                Port = "1234",
                Username = "asd",
                Password = "asd",
            };

            Assert.True(server.Validate(out _));
        }
    }
}