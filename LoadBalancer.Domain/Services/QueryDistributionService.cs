using System;
using System.Linq;
using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Domain.Storage;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;
using Npgsql;
using Quartz.Logging;

namespace LoadBalancer.Domain.Services
{
    public class QueryDistributionService : IQueryDistributionService
    {
        private readonly IStatisticsStorage _statisticsStorage;
        private readonly IResponseStorage _responseStorage;
        private readonly IQueryExecutor _queryExecutor;
        private readonly BalancerConfiguration _configuration;

        public QueryDistributionService(IStatisticsStorage statisticsStorage, IResponseStorage responseStorage, 
            IQueryExecutor queryExecutor, IOptions<BalancerConfiguration> options)
        {
            _statisticsStorage = statisticsStorage;
            _responseStorage = responseStorage;
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
                return RetryOrFail(request);
            }

            var serializedData = string.Empty;

            try
            {
                if (request.IsSelect)
                {
                    serializedData = await _queryExecutor.QueryAsync(availableServer, request.Query);
                }
                else
                {
                    await _queryExecutor.ExecuteAsync(availableServer, request.Query);
                }

                if (request.IsRetried)
                {
                    _responseStorage.Add(new Response
                    {
                        Success = true,
                        QueryData = serializedData,
                        RequestId = request.RequestId
                    });
                }

                return Ok(serializedData);
            }
            catch (Exception e)
            {
                // if not postgres error, must be system error
                // postgres errors are client-side problems
                if (e is not NpgsqlException npgsqlException) 
                    return RetryOrFail(request);
                
                if (!request.IsRetried && !request.AcceptRetries) 
                    throw npgsqlException;
                
                var maxRetryCount = _configuration.MaxRetryCount;
                if (request.CurrentRetryAttempt < maxRetryCount)
                {
                    // enqueue for retry
                    return QueuedForRetry(request.RequestId);
                }
                
                _responseStorage.Add(new Response
                {
                    Success = false,
                    ErrorMessage = "Request has failed all its attempts",
                    RequestId = request.RequestId
                });
                return Fail();
            }
        }

        private object RetryOrFail(Request request)
        {
            // failing if request does not support queue
            if (!request.AcceptRetries)
                throw new Exception("Cant execute that rn");

            var maxRetryCount = _configuration.MaxRetryCount;
            if (request.IsRetried && request.CurrentRetryAttempt >= maxRetryCount)
            {
                _responseStorage.Add(new Response
                {
                    Success = false,
                    ErrorMessage = "Request has failed all its attempts",
                    RequestId = request.RequestId
                });
                return Fail();
            }

            request.RequestId = Guid.NewGuid();
            // enqueue for retry 
            return QueuedForRetry(request.RequestId);
        }

        private static object Ok(string data = null) => new {Success = true, Data = data};
        private static object QueuedForRetry(Guid requestId) => new {Success = true, QueuedForRetry = true, RequestId = requestId };
        private static object Fail(string errorMessage = null) => new {Success = false, Message = errorMessage};
    }
}