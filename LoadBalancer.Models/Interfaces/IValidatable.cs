using System.ComponentModel.DataAnnotations;

namespace LoadBalancer.Models.Interfaces
{
    public interface IValidatable
    {
        bool Validate(out ValidationResult o);
    }
}