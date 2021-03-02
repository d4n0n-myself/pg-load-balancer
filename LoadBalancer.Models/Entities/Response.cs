using System;

namespace LoadBalancer.Models.Entities
{
    public class Response
    {
        public Guid RequestId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        //serialized
        public string QueryData { get; set; }
    }
}