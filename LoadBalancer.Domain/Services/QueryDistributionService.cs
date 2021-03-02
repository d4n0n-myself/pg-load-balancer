using System;
using System.Linq;
using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Domain.Storage;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Domain.Services
{
    public class QueryDistributionService : IQueryDistributionService
    {
        private readonly IStatisticsStorage _statisticsStorage;
        private readonly IQueryExecutor _queryExecutor;
        private readonly BalancerConfiguration _configuration;

        public QueryDistributionService(IStatisticsStorage statisticsStorage, IQueryExecutor queryExecutor,
            IOptions<BalancerConfiguration> options)
        {
            _statisticsStorage = statisticsStorage;
            _queryExecutor = queryExecutor;
            _configuration = options.Value;
        }

        public async Task<object> DistributeQuery(Request request)
        {
            var servers = _statisticsStorage.Get(request.Type);
            var maxSessions = _configuration.GetMaxSessionsParameter(request.Type);
            var (availableServer, _) = servers
                .FirstOrDefault(x => x.Value.IsOnline && x.Value.CurrentSessionsCount < maxSessions);

            if (availableServer == null)
            {
                if (!request.AcceptRetries)
                    throw new Exception("Cant execute that rn");

                var maxRetryCount = _configuration.MaxRetryCount;
                if (request.IsRetried && request.CurrentRetryAttempt >= maxRetryCount)
                {
                    // save to response storage that request has failed all its attempts
                    return Fail();
                }

                request.RequestId = Guid.NewGuid();
                // enqueue for retry 
                // return requestId to caller
                throw new NotImplementedException();
            }

            if (request.IsSelect)
            {
                var serializedData = await _queryExecutor.QueryAsync(availableServer, request.Query);
                return Ok(serializedData);
            }

            await _queryExecutor.ExecuteAsync(availableServer, request.Query);
            return Ok();
        }

        private static object Ok(string data = null) => new {Success = true, Data = data};
        private static object Fail(string errorMessage = null) => new {Success = false, Message = errorMessage};
    }
}