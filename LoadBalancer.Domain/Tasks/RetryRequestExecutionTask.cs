using System.Threading.Tasks;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Models.Enums;
using Quartz;

namespace LoadBalancer.Domain.Tasks
{
    /// <summary>
    /// Get request from queue and start its balancing and execution process.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RetryRequestExecutionTask : IJob
    {
        private readonly IQueryDistributionService _service;
        private readonly IRequestQueue _queue;
        private readonly IResponseStorage _storage;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetryRequestExecutionTask(IQueryDistributionService service, IRequestQueue queue,
            IResponseStorage storage)
        {
            _service = service;
            _queue = queue;
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context)
        {
            var request = _queue.Get();
            if (request is null)
                return;

            var response = await _service.DistributeQueryAsync(request);
            if (response.Result == QueryExecutionResult.QueryCompleted)
            {
                _storage.Add(response);
            }
        }
    }
}