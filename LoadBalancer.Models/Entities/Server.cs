// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using LoadBalancer.Models.Interfaces;

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Server configuration object.
    /// </summary>
    public class Server : IEquatable<Server>, IValidatable
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

        /// <inheritdoc />
        public bool Validate(out ValidationResult o)
        {
            if (new[] {_database, _host, _port, _username, _password}.Any(string.IsNullOrWhiteSpace))
            {
                o = new ValidationResult("Connection parameters must be set.");
                return false;
            }

            if (_host.Split('.').Length != 4)
            {
                o = new ValidationResult("Host parameter must contain 4 parts.");
                return false;
            }
            
            if (!IPAddress.TryParse(_host, out _))
            {
                o = new ValidationResult("Host parameter must be valid.");
                return false;
            }
            
            if (!int.TryParse(_port, out var port) || port <= 0)
            {
                o = new ValidationResult("Port parameter must be valid.");
                return false;
            }
            
            o = ValidationResult.Success;
            return true;
        }

        /// <inheritdoc />
        public bool Equals(Server other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _host == other._host && _port == other._port && _database == other._database &&
                   _username == other._username && _password == other._password && _name == other._name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Server) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_host, _port, _database, _username, _password, _name);
        }
        
        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(Server left, Server right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Non-Equality operator.
        /// </summary>
        public static bool operator !=(Server left, Server right)
        {
            return !Equals(left, right);
        }
    }
}