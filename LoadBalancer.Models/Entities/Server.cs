// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Server configuration object.
    /// </summary>
    public class Server
    {
        private readonly string _host;
        private readonly string _port;
        private readonly string _database;
        private readonly string _username;
        private readonly string _password;
        private readonly string _name;

        /// <summary>
        /// IP address. Defaults to localhost.
        /// </summary>
        public string Host
        {
            get => _host ?? "localhost";
            init => _host = value;
        }

        /// <summary>
        /// Host port. Defaults to 5432.
        /// </summary>
        public string Port
        {
            get => _port ?? "5432"; 
            init => _port = value;
        }

        /// <summary>
        /// Database. Defaults to postgres.
        /// </summary>
        public string Database
        {
            get => _database ?? "postgres";
            init => _database = value;
        }

        /// <summary>
        /// Username. Defaults to postgres.
        /// </summary>
        public string Username
        {
            get => _username ?? "postgres";
            init => _username = value;
        }

        /// <summary>
        /// Password. Defaults to postgres.
        /// </summary>
        public string Password
        {
            get => _password ?? "postgres";
            init => _password = value;
        }
        
        /// <summary>
        /// Human name of a server. If no value provided, <see cref="Name"/> == <see cref="Host"/>.
        /// </summary>
        public string Name
        {
            get => _name ?? _host;
            init => _name = value;
        }

        /// <summary>
        /// Retrieve server configuration as connection string for pg connections.
        /// </summary>
        public string AsConnectionString()
        {
            return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
        }
    }
}