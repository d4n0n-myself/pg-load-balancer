using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.Interfaces;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Sql query response object embodiment.
    /// </summary>
    public class Response : IEquatable<Response>, IValidatable
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
        /// Error messages, PG exceptions mostly. Is not empty when <see cref="Result"/> == <see cref="QueryExecutionResult.QueryFailed"/>.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Query response (serialized). 
        /// </summary>
        public string Data { get; init; }

        /// <summary>
        /// Return default response for completed situation.
        /// </summary>
        public static Response Completed(string data = null, Guid? requestId = null) =>
            new()
            {
                Result = QueryExecutionResult.QueryCompleted, 
                Data = data,
                RequestId = requestId.HasValue && requestId.Value != Guid.Empty ? requestId : null
            };

        /// <summary>
        /// Return default response for queued situation.
        /// </summary>
        public static Response Queued(Guid requestId) =>
            new() {Result = QueryExecutionResult.QueryQueued, RequestId = requestId};

        /// <summary>
        /// Return default response for fail situation.
        /// </summary>
        public static Response Fail(string errorMessage = null, Guid? requestId = null) =>
            new()
            {
                Result = QueryExecutionResult.QueryFailed,
                Message = errorMessage,
                RequestId = requestId.HasValue && requestId.Value != Guid.Empty ? requestId : null
            };

        /// <inheritdoc />
        public bool Validate(out ValidationResult o)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (Result == 0)
            {
                o = new ValidationResult("Result type must be set.");
                return false;
            }
            
            if (Result == QueryExecutionResult.QueryFailed && string.IsNullOrWhiteSpace(Message))
            {
                o = new ValidationResult("Error message must be set.");
                return false;
            }

            o = ValidationResult.Success;
            return true;
        }

        /// <inheritdoc />
        public bool Equals(Response other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Nullable.Equals(RequestId, other.RequestId) && Result == other.Result && Message == other.Message &&
                   Data == other.Data;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Response) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(RequestId, (int) Result, Message, Data);
        }
        
        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(Response left, Response right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Non-Equality operator.
        /// </summary>
        public static bool operator !=(Response left, Response right)
        {
            return !Equals(left, right);
        }
    }
}