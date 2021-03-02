using System;
using System.Threading.Tasks;
using Dapper;
using LoadBalancer.Models.Entities;
using Npgsql;
using static System.Text.Json.JsonSerializer;

namespace LoadBalancer.Database.Query
{
    public class QueryExecutor : IQueryExecutor, IDisposable
    {
        public async Task ExecuteAsync(Server server, string query)
        {
            await using var npgsqlConnection = new NpgsqlConnection(server.AsConnectionString());
            await using var npgsqlTransaction = await npgsqlConnection.BeginTransactionAsync();
            try
            {
                await npgsqlConnection.ExecuteAsync(query);
                await npgsqlTransaction.CommitAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to execute query {query} for server {server.Host}: {e.Message}");
            }
        }

        public async Task<string> QueryAsync(Server server, string query)
        {
            await using var npgsqlConnection = new NpgsqlConnection(server.AsConnectionString());
            await using var npgsqlTransaction = await npgsqlConnection.BeginTransactionAsync();
            try
            {
                var results = await npgsqlConnection.QueryAsync(query);
                await npgsqlTransaction.CommitAsync();
                var serialized = Serialize(results);
                return serialized;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to query {query} for server {server.Host}: {e.Message}");
            }
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}