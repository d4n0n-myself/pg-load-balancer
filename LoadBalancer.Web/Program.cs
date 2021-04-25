using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace LoadBalancer.Web
{
    /// <summary>
    /// Main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main.
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(
                    builder =>
                    {
                        builder.ClearProviders();
                        builder.AddNLog();
                    })
            ;
    }
}