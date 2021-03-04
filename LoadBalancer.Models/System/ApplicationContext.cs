using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LoadBalancer.Models.System
{
    /// <summary>
    /// Application configuration holder object.
    /// </summary>
    public class ApplicationContext
    {
        /// <summary>
        /// Current application configuration.
        /// </summary>
        public static ApplicationContext Current { get; set; }

        /// <summary>
        /// Dependency injection container.
        /// </summary>
        public IServiceProvider Container { get; init; }
    }
}