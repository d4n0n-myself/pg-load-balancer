using System.Threading.Tasks;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Services
{
    public interface IQueryDistributionService
    {
        Task<object> DistributeQuery(Request request);
    }
}