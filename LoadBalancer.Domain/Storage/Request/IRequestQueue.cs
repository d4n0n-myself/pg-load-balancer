namespace LoadBalancer.Domain.Storage.Request
{
    /// <summary>
    /// A queue for <see cref="Models.Entities.Request"/>.
    /// </summary>
    public interface IRequestQueue
    {
        /// <summary>
        /// Push to queue.
        /// </summary>
        void Add(Models.Entities.Request request);

        /// <summary>
        /// Pop from queue.
        /// </summary>
        Models.Entities.Request Get();

        /// <summary>
        /// Purge the queue full. 
        /// </summary>
        void Purge();
    }
}