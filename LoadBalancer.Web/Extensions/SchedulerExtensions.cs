using System.Threading.Tasks;
using LoadBalancer.Domain;
using Quartz;

namespace LoadBalancer.Web.Extensions
{
    internal static class SchedulerExtensions
    {
        internal static async Task RegisterJobAsync(this IScheduler scheduler,
            int intervalInSeconds,
            string taskPrefix,
            string jobDescription = "")
        {
            var job = JobBuilder.Create<RetrieveOlapStatisticsTask>()
                .WithIdentity($"{taskPrefix}Task")
                .RequestRecovery()
                .WithDescription(jobDescription)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{taskPrefix}Trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(intervalInSeconds)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}