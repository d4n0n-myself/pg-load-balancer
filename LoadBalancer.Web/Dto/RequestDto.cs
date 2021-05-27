using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Enums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LoadBalancer.Web.Dto
{
    /// <summary>
    /// Request data transfer object.
    /// </summary>
    public class RequestDto
    {
        /// <summary>
        /// Query type - Olap / Oltp. 
        /// </summary>
        [Required]
        public QueryType Type { get; set; }

        /// <summary>
        /// Will contain data to return.
        /// </summary>
        [Required]
        public bool IsSelect { get; set; }

        /// <summary>
        /// Raw sql query.
        /// </summary>
        [Required]
        public string Query { get; set; }

        /// <summary>
        /// Client can accept retries for this request. Result will be stored in response storage.
        /// </summary>
        [Required]
        public bool AcceptRetries { get; set; }

        /// <summary>
        /// Priority of retrying request. Bigger the value = more priority. 
        /// </summary>
        public int Priority { get; set; }
    }
}