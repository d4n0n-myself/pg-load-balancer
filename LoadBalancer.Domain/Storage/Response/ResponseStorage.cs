using System;
using System.Collections.Concurrent;
using QueryResponse = LoadBalancer.Models.Entities.Response;

namespace LoadBalancer.Domain.Storage.Response
{
    /// <inheritdoc />
    public class ResponseStorage : IResponseStorage
    {
        private readonly ConcurrentDictionary<Guid, QueryResponse> _values = new();

        /// <inheritdoc />
        public bool TryGetResponseByRequestId(Guid requestId, out QueryResponse response)
        {
            return _values.TryRemove(requestId, out response);
        }

        /// <inheritdoc />
        public void Add(QueryResponse response)
        {
            if (response.RequestId is null || response.RequestId == Guid.Empty)
            {
                throw new ArgumentException("No request id provided to save response!");
            }

            _values.AddOrUpdate(response.RequestId.Value, response, (_, _) => response);
        }
    }
}