using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Enums;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Sql query request object embodiment.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Query type - Olap / Oltp. 
        /// </summary>
        [Required] public QueryType Type { get; set; }
        
        /// <summary>
        /// Will contain data to return.
        /// </summary>
        [Required] public bool IsSelect { get; set; }
        
        /// <summary>
        /// Raw sql query.
        /// </summary>
        [Required] public string Query { get; set; }
        
        /// <summary>
        /// Client can accept retries for this request. Result will be stored in response storage.
        /// </summary>
        [Required] public bool AcceptRetries { get; set; }
        
        /// <summary>
        /// Priority of retrying request. Bigger the value = more priority. 
        /// </summary>
        public int Priority { get; set; }
        
        /// <summary>
        /// Identifier. Is being given to client to get response if request if accepting retries. 
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// Request is executing not the first time. (Is being taken from queue)
        /// </summary>
        public bool IsRetried { get; set; }
        
        /// <summary>
        /// Attempt limiter.
        /// </summary>
        public int CurrentRetryAttempt { get; set; } = 0;
    }
}