using System;

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Sql query response object embodiment.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Identifier for client to retrieve response by.
        /// </summary>
        public Guid RequestId { get; set; }
        
        /// <summary>
        /// Query execution result.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Error messages, PG exceptions mostly. Is not empty when <see cref="Success"/> == false.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Query response (serialized). 
        /// </summary>
        public string QueryData { get; set; }
    }
}