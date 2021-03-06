using System;
using System.Collections.Concurrent;

namespace LoadBalancer.Domain.Storage.Response
{
    public class ResponseStorage : IResponseStorage
    {
        private readonly ConcurrentDictionary<Guid, Models.Entities.Response> _values = new();

        public bool TryGetResponseByRequestId(Guid requestId, out Models.Entities.Response response)
        {
            return _values.TryGetValue(requestId, out response);
        }

        public void Add(Models.Entities.Response response)
        {
            if (response.RequestId is null || response.RequestId == Guid.Empty)
            {
                throw new ArgumentException("No request id provided to save response!");
            }

            _values.AddOrUpdate(response.RequestId.Value, response, (_, _) => response);
        }
    }
}