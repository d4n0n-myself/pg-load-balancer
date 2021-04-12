using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LoadBalancer.Models.System
{
    /// <summary>
    /// Application configuration holder object.
    /// </summary>
    public static class ApplicationContext
    {
        /// <summary>
        /// Dependency injection container.
        /// </summary>
        public static IServiceProvider Container { get; set; }
    }
}