using System;
using Microsoft.Extensions.DependencyInjection;

namespace LoadBalancer.Tests.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider provider)
        {
            var service = provider.GetService<T>();
            if (service is null)
                throw new ResolveException();
            return service;
        }
    }

    internal class ResolveException : Exception
    {
        
    }
}