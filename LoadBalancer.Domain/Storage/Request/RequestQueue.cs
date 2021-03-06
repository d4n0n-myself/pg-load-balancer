using System;
using System.Collections.Concurrent;
using System.Linq;

namespace LoadBalancer.Domain.Storage.Request
{
    /// <inheritdoc />
    public class RequestQueue : IRequestQueue
    {
        private readonly ConcurrentDictionary<(Guid, DateTime), Models.Entities.Request> _values = new();

        /// <inheritdoc />
        public void Add(Models.Entities.Request request)
        {
            _values.AddOrUpdate((request.RequestId, DateTime.Now), request, (_, _) => request);
        }

        /// <inheritdoc />
        public Models.Entities.Request Get()
        {
            if (_values.IsEmpty)
            {
                return null;
            }
            
            var minPriority = _values.Max(x => x.Value.Priority);
            var request = _values.OrderByDescending(x => x.Key)
                .First(x => x.Value.Priority == minPriority);
            if (!_values.TryRemove(request))
            {
                throw new Exception("Can't remove data");
            }
            return request.Value;
        }

        /// <inheritdoc />
        public void Purge()
        {
            while (Get() is not null) { }
        }
    }
}