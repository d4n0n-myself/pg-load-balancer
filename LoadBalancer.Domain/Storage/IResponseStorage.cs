using System;

namespace LoadBalancer.Domain.Storage
{
    public interface IResponseStorage
    {
        bool TryGetResponseByRequestId(Guid requestId, out object response);
    }
}