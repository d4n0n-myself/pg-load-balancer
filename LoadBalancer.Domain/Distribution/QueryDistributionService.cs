using System;
using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Domain.Decision;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Domain.Storage.Response;
using LoadBalancer.Domain.Storage.Statistics;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.System;
using Microsoft.Extensions.Options;
using Npgsql;

namespace LoadBalancer.Domain.Distribution
{
    /// <inheritdoc />
    public class QueryDistributionService : IQueryDistributionService
    {
        private readonly IStatisticsStorage _statisticsStorage;
        private readonly IQueryExecutor _queryExecutor;
        private readonly BalancerConfiguration _configuration;
        private readonly IResponseStorage _responseStorage;
        private readonly IRequestQueue _queue;
        private readonly IServerDecider _serverDecider;

        /// <summary>
        /// Constructor.
        /// </summary>
        public QueryDistributionService(IStatisticsStorage statisticsStorage, IResponseStorage responseStorage, 
            IOptions<BalancerConfiguration> options, IQueryExecutor queryExecutor, IRequestQueue queue, IServerDecider decider)
        {
            _statisticsStorage = statisticsStorage;
            _queryExecutor = queryExecutor;
            _responseStorage = responseStorage;
            _queue = queue;
            _configuration = options.Value;
            _serverDecider = decider;
        }

        /// <inheritdoc />
        public async Task<Response> DistributeQueryAsync(Request request)
        {
            if (!request.Validate(out var validationResult))
            {
                return Response.Fail(validationResult.ErrorMessage);
            }
            
            var servers = _statisticsStorage.Get(request.Type);
            var maxSessions = _configuration.GetMaxSessionsParameter(request.Type);
            var availableServer = _serverDecider.FindAvailableServer(servers, maxSessions);

            if (availableServer is null)
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

                return Response.Completed(serializedData, request.RequestId);
            }
            catch (PostgresException postgresException)
            {
                var message = $"Query is not correct: {postgresException.Message}";
                // postgres errors are client-side problems
                if (!request.AcceptRetries)
                    return Response.Fail(message);

                request.RequestId = Guid.NewGuid();
                _responseStorage.Add(Response.Fail(message, request.RequestId));
                return Response.Queued(request.RequestId);
            }
            catch (Exception)
            {
                // if not postgres error, must be system error

                // failing if request does not support queue
                if (!request.AcceptRetries)
                    return Response.Fail("No server available right now.");

                var maxRetryCount = _configuration.MaxRetryCount;
                if (request.IsRetried && request.CurrentRetryAttempt >= maxRetryCount)
                {
                    _responseStorage.Add(Response.Fail("Number of allowed attempts exceeded", request.RequestId));
                    return Response.Fail();
                }

                request.IsRetried = true;
                request.RequestId = Guid.NewGuid();
                _queue.Add(request);
                return Response.Completed(request.RequestId.ToString());
            }
        }

        private Response HandleNoAvailableServerScenario(Request request)
        {
            // failing if request does not support queue
            if (!request.AcceptRetries)
                return Response.Fail("No server available right now.");

            var maxRetryCount = _configuration.MaxRetryCount;
            if (request.IsRetried && request.CurrentRetryAttempt >= maxRetryCount)
            {
                _responseStorage.Add(Response.Fail("Number of allowed attempts exceeded", request.RequestId));
                return Response.Fail();
            }

            request.IsRetried = true;
            request.RequestId = Guid.NewGuid();
            _queue.Add(request);
            return Response.Queued(request.RequestId);
        }
    }
}