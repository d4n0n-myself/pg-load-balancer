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
            await Run(server, query, async connection =>
            {
                await connection.ExecuteAsync(query);
                return string.Empty;
            });
        }

        public async Task<string> QueryAsync(Server server, string query)
        {
            return await Run(server, query, async connection =>
            {
                var results = await connection.QueryAsync(query);
                var serialized = Serialize(results);
                return serialized;
            });
        }

        private static async Task<string> Run(Server server, string query, Func<NpgsqlConnection, Task<string>> action)
        {
            await using var connection = new NpgsqlConnection(server.AsConnectionString());
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var result = await action(connection);
                await transaction.CommitAsync();
                return result;
            }
            catch (PostgresException)
            {
                throw;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Failed to execute query {query} for server {server.Host}: {e.Message}");
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}