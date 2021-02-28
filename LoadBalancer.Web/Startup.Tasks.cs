using System;
using System.Threading.Tasks;
using LoadBalancer.Models;
using LoadBalancer.Web.Extensions;
using LoadBalancer.Web.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz.Impl;

namespace LoadBalancer.Web
{
    public partial class Startup
    {
        private async Task RegisterTasks(IServiceProvider container)
        {
            var balancerConfiguration = container.GetService<IOptions<BalancerConfiguration>>()?.Value;
            if (balancerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(balancerConfiguration), "Balancing options required to start!");
            }

            var jobFactory = new ContainerJobFactory(container);

            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler()
                .ConfigureAwait(false).GetAwaiter().GetResult();
            
            scheduler.JobFactory = jobFactory;

            await scheduler.Start();

            var olapRefreshInterval = balancerConfiguration.RefreshOlapStatisticsIntervalInSec;
            var oltpRefreshInterval = balancerConfiguration.RefreshOltpStatisticsIntervalInSec;

            await scheduler.RegisterJobAsync(olapRefreshInterval, "olapStats",
                "Request statistics from PostgreSQL server");
            await scheduler.RegisterJobAsync(oltpRefreshInterval, "oltpStats",
                "Request statistics from PostgreSQL server");
        }
    }
}