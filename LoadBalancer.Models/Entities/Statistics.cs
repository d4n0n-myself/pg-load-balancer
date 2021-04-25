using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Interfaces;

namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Server statistics holder object.
    /// </summary>
    public class Statistics : IEquatable<Statistics>, IValidatable
    {
        /// <summary>
        /// Server is online and responding to "GetStatistics" query.
        /// </summary>
        public bool IsOnline { get; init; }

        /// <summary>
        /// Active transactions count on the server.
        /// </summary>
        public int CurrentSessionsCount { get; init; }

        /// <summary>
        /// Convenient handler: get empty stats object { online = false, current sessions = 0 }.
        /// </summary>
        public static Statistics Empty => new();

        /// <inheritdoc />
        public bool Validate(out ValidationResult o)
        {
            if (CurrentSessionsCount < 0)
            {
                o = new ValidationResult("Count must be positive.");
                return false;
            }

            o = ValidationResult.Success;
            return true;
        }

        /// <inheritdoc />
        public bool Equals(Statistics other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsOnline == other.IsOnline && CurrentSessionsCount == other.CurrentSessionsCount;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Statistics) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(IsOnline, CurrentSessionsCount);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(Statistics left, Statistics right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Non-Equality operator.
        /// </summary>
        public static bool operator !=(Statistics left, Statistics right)
        {
            return !Equals(left, right);
        }
    }
}