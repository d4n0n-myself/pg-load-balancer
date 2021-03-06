using System.Threading.Tasks;
using LoadBalancer.Domain.Services;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Models.Enums;
using Quartz;

namespace LoadBalancer.Domain.Tasks
{
    public class RetryRequestExecutionTask : IJob
    {
        private readonly IQueryDistributionService _service;
        private readonly IRequestQueue _queue;
        private readonly IResponseStorage _storage;

        public RetryRequestExecutionTask(IQueryDistributionService service, IRequestQueue queue,
            IResponseStorage storage)
        {
            _service = service;
            _queue = queue;
            _storage = storage;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var request = _queue.Get();
            if (request == null)
                return;

            var response = await _service.DistributeQueryAsync(request);
            if (response.Result == QueryExecutionResult.QueryCompleted)
            {
                _storage.Add(response);
            }
        }
    }
}