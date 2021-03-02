using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Database.Query
{
    public interface IQueryExecutor
    {
        Task ExecuteAsync(Server server, string query);
        Task<string> QueryAsync(Server server, string query);
    }
}