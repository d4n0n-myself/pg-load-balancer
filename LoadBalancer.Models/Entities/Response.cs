using System;
using LoadBalancer.Models.Enums;

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
        public Guid? RequestId { get; set; }
        
        /// <summary>
        /// Query execution result.
        /// </summary>
        public QueryExecutionResult Result { get; init; }
        
        /// <summary>
        /// Error messages, PG exceptions mostly. Is not empty when <see cref="Success"/> == false.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Query response (serialized). 
        /// </summary>
        public string Data { get; init; }
        
        public static Response Completed(string data = null) =>
            new() {Result = QueryExecutionResult.QueryCompleted, Data = data};

        public static Response Queued(Guid requestId) =>
            new() {Result = QueryExecutionResult.QueryQueued, RequestId = requestId};

        public static Response Fail(string errorMessage = null, Guid? requestId = null) =>
            new()
            {
                Result = QueryExecutionResult.QueryFailed,
                Message = errorMessage,
                RequestId = requestId
            };
    }
}