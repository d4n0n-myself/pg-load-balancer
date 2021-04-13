using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Tests.Extensions
{
    /// <summary>
    /// <see cref="IServiceProvider"/> extensions class.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Resolve service from native service provider, otherwise throw an <see cref="ResolveException"/>.
        /// </summary>
        /// <exception cref="ResolveException"></exception>
        public static T Resolve<T>(this IServiceProvider provider)
        {
            var service = provider.GetService<T>();
            if (service is null)
                throw new ResolveException();
            return service;
        }

        /// <summary>
        /// Resolve configuration from native service provider, otherwise throw an <see cref="ResolveException"/>.
        /// </summary>
        /// <exception cref="ResolveException"></exception>
        public static T Configuration<T>(this IServiceProvider provider) where T : class
        {
            var service = provider.GetService<IOptions<T>>();
            if (service?.Value is null)
                throw new ResolveException();
            return service.Value;
        }
    }

    /// <summary>
    /// Exception that is being thrown when resolving service which is not found in container.
    /// </summary>
    internal class ResolveException : Exception
    {
        
    }
}