using System.Threading.Tasks;
using LoadBalancer.Models;

namespace LoadBalancer.Database
{
    public interface IStatisticsRepository
    {
        Task<Statistics> GetStatistics(Server server);
    }
}