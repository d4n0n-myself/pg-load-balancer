using System.Threading.Tasks;
using Quartz;

namespace LoadBalancer.Web.Extensions
{
    /// <summary>
    /// Quartz tasks scheduler extensions.
    /// </summary>
    internal static class SchedulerExtensions
    {
        /// <summary>
        /// Register domain task with given interval.
        /// </summary>
        internal static async Task RegisterJobAsync<T>(this IScheduler scheduler,
            int intervalInSeconds,
            string taskPrefix,
            string jobDescription = "") where T : IJob
        {
            var job = JobBuilder.Create<T>()
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