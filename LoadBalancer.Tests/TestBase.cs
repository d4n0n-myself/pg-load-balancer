using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Dapper;
using LoadBalancer.Database.Query;
using LoadBalancer.Database.Statistics;
using LoadBalancer.Domain.Decision;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.System;
using LoadBalancer.Tests.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Base test class. Compiles the service provider for tests.
    /// </summary>
    public class TestBase
    {
        private readonly string _configFilePath =
            Directory.GetCurrentDirectory() + "../../../../appsettings.json";

        /// <summary>
        /// DI container.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// ctor.
        /// </summary>
        protected TestBase()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(_configFilePath)
                .Build();

            services.Configure<BalancerConfiguration>(configuration);

            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            services.AddScoped<IRequestQueue, RequestQueue>();
            services.AddScoped<IResponseStorage, ResponseStorage>();
            services.AddScoped<IStatisticsStorage, StatisticsStorage>();

            services.AddScoped<IServerDecider, ServerDecider>();
            services.AddScoped<IQueryDistributionService, QueryDistributionService>();

            ServiceProvider = services.BuildServiceProvider();

            ApplicationContext.Container = ServiceProvider;
        }

        /// <summary>
        /// Add statistics (Online=true,SessionsCount) to chosen storage.
        /// </summary>
        protected static void SetLocalhostStatisticsAsOnline(IStatisticsStorage storage, int sessionsCount = 1)
        {
            storage.Set(QueryType.Oltp, new Server(), new Statistics {IsOnline = true, CurrentSessionsCount = sessionsCount});
        }

        /// <summary>
        /// Simulate load on database server by opening <paramref name="connectionCount"></paramref> connections.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="connectionCount"></param>
        protected void SimulateDbLoad(Action action, int connectionCount = 3)
        {
            IEnumerable<NpgsqlConnection> connections = ImmutableArray<NpgsqlConnection>.Empty;
            try
            {
                var configuration = ServiceProvider.Resolve<BalancerConfiguration>();
                connections = Enumerable.Range(0, connectionCount).Select(_ =>
                {
                    var connection = new NpgsqlConnection(configuration.OltpPool.Single().AsConnectionString());
                    connection.Open();
                    connection.Execute("SELECT 1");
                    return connection;
                });
            }
            finally
            {
                foreach (var connection in connections)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}