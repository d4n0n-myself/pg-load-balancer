using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace LoadBalancer.Web.Factories
{
    /// <summary>
    /// Quartz task factory with dependency injection and scopes.
    /// </summary>
    internal class ContainerJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new();

        public ContainerJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();
            IJob job;

            try
            {
                job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            catch
            {
                scope.Dispose();
                throw;
            }

            if (_scopes.TryAdd(job, scope)) 
                return job;
            
            scope.Dispose();
            throw new Exception("Failed to track DI scope");
        }

        public void ReturnJob(IJob job)
        {
            if (_scopes.TryRemove(job, out var scope))
            {
                scope.Dispose();
            }
        }
    }
}