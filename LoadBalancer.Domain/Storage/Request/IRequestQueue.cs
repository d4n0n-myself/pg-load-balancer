namespace LoadBalancer.Domain.Storage.Request
{
    public interface IRequestQueue
    {
        void Add(Models.Entities.Request request);
        Models.Entities.Request Get();
    }
}