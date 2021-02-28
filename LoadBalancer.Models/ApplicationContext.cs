using System;

namespace LoadBalancer.Models
{
    public class ApplicationContext
    {
        public static ApplicationContext Current { get; set; }

        public IServiceProvider Container { get; set; }
    }
}