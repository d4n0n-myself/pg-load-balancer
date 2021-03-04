using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Services
{
    /// <summary>
    /// Service to identify if query can be processed and define server to process query.
    /// </summary>
    public interface IQueryDistributionService
    {
        /// <summary>
        /// Determine server and execute query.
        /// </summary>
        Task<object> DistributeQuery(Request request);
    }
}