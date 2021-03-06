using System;
using System.Collections.Concurrent;
using QueryResponse = LoadBalancer.Models.Entities.Response;

namespace LoadBalancer.Domain.Storage.Response
{
    public class ResponseStorage : IResponseStorage
    {
        private readonly ConcurrentDictionary<Guid, QueryResponse> _values = new();

        public bool TryGetResponseByRequestId(Guid requestId, out QueryResponse response)
        {
            return _values.TryGetValue(requestId, out response);
        }

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