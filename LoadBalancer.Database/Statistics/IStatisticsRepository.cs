using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Database.Statistics
{
    /// <summary>
    /// Repository to retrieve statistics.
    /// </summary>
    public interface IStatisticsRepository
    {
        /// <summary>
        /// Access <param name="server"></param> and get statistics.
        /// </summary>
        Task<Models.Entities.Statistics> GetStatistics(Server server);
    }
}