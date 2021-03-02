// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace LoadBalancer.Models.Entities
{
    public class Server
    {
        private readonly string _host;
        private readonly string _port;
        private readonly string _database;
        private readonly string _username;
        private readonly string _password;
        private readonly string _name;

        public string Host
        {
            get => _host ?? "localhost";
            init => _host = value;
        }

        public string Port
        {
            get => _port ?? "5432"; 
            init => _port = value;
        }

        public string Database
        {
            get => _database ?? "postgres";
            init => _database = value;
        }

        public string Username
        {
            get => _username ?? "postgres";
            init => _username = value;
        }

        public string Password
        {
            get => _password ?? "postgres";
            init => _password = value;
        }
        
        public string Name
        {
            get => _name ?? _host;
            init => _name = value;
        }

        public string AsConnectionString()
        {
            return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
        }
    }
}