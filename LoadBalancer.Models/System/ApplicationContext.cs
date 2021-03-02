using System;

namespace LoadBalancer.Models.System
{
    public class ApplicationContext
    {
        public static ApplicationContext Current { get; set; }

        public IServiceProvider Container { get; set; }
    }
}