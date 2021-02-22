using System;

namespace LoadBalancer.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public QueryType Type { get; set; }
        public int Priority { get; set; }
        public bool AcceptRetries { get; set; }
        public string Query { get; set; }
        public bool IsRetried { get; set; }
        public int CurrentRetryAttempt { get; set; } = 0;
    }

    public enum QueryType
    {
        Olap = 1,
        Oltp = 2
    }
}