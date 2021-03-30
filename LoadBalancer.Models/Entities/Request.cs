using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Enums;
using LoadBalancer.Models.Interfaces;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Sql query request object embodiment.
    /// </summary>
    public class Request : IEquatable<Request>, IValidatable
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
        public int CurrentRetryAttempt { get; set; }

        public bool Validate(out ValidationResult o)
        {
            if (CurrentRetryAttempt < 0)
            {
                o = new ValidationResult("Current attempt must be positive.");
                return false;
            }

            if (IsRetried && RequestId == Guid.Empty)
            {
                o = new ValidationResult("Request id must be set to do retries.");
                return false;
            }
            
            if (string.IsNullOrEmpty(Query))
            {
                o = new ValidationResult("Query cannot be empty.");
                return false;
            }

            if (Type == 0)
            {
                o = new ValidationResult("Type must be defined");
                return false;
            }

            o = ValidationResult.Success;
            return true;
        }

        public bool Equals(Request other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && IsSelect == other.IsSelect && Query == other.Query &&
                   AcceptRetries == other.AcceptRetries && Priority == other.Priority &&
                   RequestId.Equals(other.RequestId) && IsRetried == other.IsRetried &&
                   CurrentRetryAttempt == other.CurrentRetryAttempt;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Request) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Type, IsSelect, Query, AcceptRetries, Priority, RequestId, IsRetried,
                CurrentRetryAttempt);
        }

        public static bool operator ==(Request left, Request right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Request left, Request right)
        {
            return !Equals(left, right);
        }
    }
}