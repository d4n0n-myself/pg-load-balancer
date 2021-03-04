using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Database.Query
{
    /// <summary>
    /// Sql query runner.
    /// </summary>
    public interface IQueryExecutor
    {
        /// <summary>
        /// Run sql query without returning data.
        /// </summary>
        /// <example>INSERT INTO my_table (col1, col2) VALUES (val1, val2)</example>
        Task ExecuteAsync(Server server, string query);
        
        /// <summary>
        /// Run sql query with returning data.
        /// </summary>
        /// <example>SELECT * FROM my_table</example>
        Task<string> QueryAsync(Server server, string query);
    }
}