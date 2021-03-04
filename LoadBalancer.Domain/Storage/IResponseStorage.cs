using System;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Storage
{
    /// <summary>
    /// In-memory response storage to store responses from queries that were executed from queue.
    /// </summary>
    public interface IResponseStorage
    {
        /// <summary>
        /// Get response by request id.
        /// </summary>
        /// <remarks>
        /// Supposed to be used to retrieve responses to queries which were executed from queue.
        /// </remarks>
        bool TryGetResponseByRequestId(Guid requestId, out object response);
        
        /// <summary>
        /// Add query response to storage.
        /// </summary>
        /// <remarks>
        /// Intended to be called after completing query from queue.
        /// </remarks>
        void Add(Response response);
    }
}