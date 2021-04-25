using System.ComponentModel.DataAnnotations;

namespace LoadBalancer.Models.Interfaces
{
    /// <summary>
    /// Entity that could be validatable.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validate given entity.
        /// </summary>
        bool Validate(out ValidationResult o);
    }
}