using System;

namespace LoadBalancer.Domain.Storage.Response
{
    public interface IResponseStorage
    {
        bool TryGetResponseByRequestId(Guid requestId, out Models.Entities.Response response);

        void Add(Models.Entities.Response response);
    }
}