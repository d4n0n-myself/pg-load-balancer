using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Database.Statistics
{
    public interface IStatisticsRepository
    {
        Task<Models.Entities.Statistics> GetStatistics(Server server);
    }
}