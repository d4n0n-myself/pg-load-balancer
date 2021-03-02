using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Enums;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LoadBalancer.Models.Entities
{
    public class Request
    {
        [Required] public QueryType Type { get; set; }
        [Required] public bool IsSelect { get; set; }
        [Required] public string Query { get; set; }
        
        [Required] public bool AcceptRetries { get; set; }
        public int Priority { get; set; }
        public Guid RequestId { get; set; }

        public bool IsRetried { get; set; }
        public int CurrentRetryAttempt { get; set; } = 0;
    }
}