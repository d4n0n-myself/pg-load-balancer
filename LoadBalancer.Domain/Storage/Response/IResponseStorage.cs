using System;

namespace LoadBalancer.Domain.Storage.Response
{
    /// <summary>
    /// A storage for <see cref="Models.Entities.Response"/>
    /// </summary>
    public interface IResponseStorage
    {
        /// <summary>
        /// Try to pop a value from response storage.
        /// </summary>
        bool TryGetResponseByRequestId(Guid requestId, out Models.Entities.Response response);

        /// <summary>
        /// Add <see cref="Models.Entities.Response"/> to response storage.
        /// </summary>
        void Add(Models.Entities.Response response);
    }
}