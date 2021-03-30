using System;
using System.Linq;
using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;
using Npgsql;

namespace LoadBalancer.Domain.Services
{
    public class QueryDistributionService : IQueryDistributionService
    {
        private readonly IStatisticsStorage _statisticsStorage;
        private readonly IQueryExecutor _queryExecutor;
        private readonly BalancerConfiguration _configuration;
        private readonly IResponseStorage _responseStorage;
        private readonly IRequestQueue _queue;

        public QueryDistributionService(IStatisticsStorage statisticsStorage, IQueryExecutor queryExecutor,
            IOptions<BalancerConfiguration> options, IResponseStorage responseStorage, IRequestQueue queue)
        {
            _statisticsStorage = statisticsStorage;
            _queryExecutor = queryExecutor;
            _responseStorage = responseStorage;
            _queue = queue;
            _configuration = options.Value;
        }

        public async Task<Response> DistributeQueryAsync(Request request)
        {
            if (!request.Validate(out var validationResult))
            {
                return Response.Fail(validationResult.ErrorMessage);
            }
            
            var servers = _statisticsStorage.Get(request.Type);
            var maxSessions = _configuration.GetMaxSessionsParameter(request.Type);
            var (availableServer, _) = servers
                .FirstOrDefault(x => x.Value.IsOnline && x.Value.CurrentSessionsCount < maxSessions);

            if (availableServer == null)
            {
                return HandleNoAvailableServerScenario(request);
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
                    _responseStorage.Add(Response.Completed(serializedData, request.RequestId));
                }

                return Response.Completed(serializedData);
            }
            catch (Exception e)
            {
                // if not postgres error, must be system error
                // postgres errors are client-side problems
                if (e is not NpgsqlException npgsqlException)
                    return HandleNoAvailableServerScenario(request);

                if (!request.IsRetried && !request.AcceptRetries)
                    throw npgsqlException;

                var maxRetryCount = _configuration.MaxRetryCount;
                if (request.CurrentRetryAttempt < maxRetryCount)
                {
                    // enqueue for retry
                    return Response.Queued(request.RequestId);
                }

                var response = Response.Fail("Request has failed all its attempts", request.RequestId);
                _responseStorage.Add(response);
                return response;
            }
        }

        private Response HandleNoAvailableServerScenario(Request request)
        {
            // failing if request does not support queue
            if (!request.AcceptRetries)
                throw new Exception("Cant execute that rn");

            var maxRetryCount = _configuration.MaxRetryCount;
            if (request.IsRetried && request.CurrentRetryAttempt >= maxRetryCount)
            {
                _responseStorage.Add(Response.Fail("Number of allowed attempts exceeded", request.RequestId));
                return Response.Fail();
            }


            request.RequestId = Guid.NewGuid();
            _queue.Add(request);
            return Response.Completed(request.RequestId.ToString());
        }
    }
}