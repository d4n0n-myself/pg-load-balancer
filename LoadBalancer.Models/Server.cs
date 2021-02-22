using System.Data.Common;

namespace LoadBalancer.Models
{
    public class Server
    {
        private Server()
        {
        }

        public Server(string connectionString)
        {
            var builder = new DbConnectionStringBuilder {ConnectionString = connectionString};
            Host = (string) builder["Host"];
            Port = (string) builder["Port"];
            Database = (string) builder["Database"];
            Username = (string) builder["Username"];
            Password = (string) builder["Password"];
            _connectionString = connectionString;
        }
        
        public string Host { get; private init; }
        public string Port { get; private init; }
        public string Database { get; private init; }
        public string Username { get; private init; }
        public string Password { get; private init; }

        private string _connectionString;

        public static Server FromConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder {ConnectionString = connectionString};
            var server = new Server
            {
                Host = (string) builder["Host"],
                Port = (string) builder["Port"],
                Database = (string) builder["Database"],
                Username = (string) builder["Username"],
                Password = (string) builder["Password"],
                _connectionString = connectionString
            };
            return server;
        }

        public string AsConnectionString()
        {
            return _connectionString;
        }
    }
}