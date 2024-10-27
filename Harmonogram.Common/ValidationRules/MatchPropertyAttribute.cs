using System.ComponentModel.DataAnnotations;

namespace Harmonogram.Common.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MatchPropertyAttribute(string otherPropertyName) : ValidationAttribute
    {
        private readonly string _otherPropertyName = otherPropertyName;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (property == null)
            {
                throw new ArgumentException($"Właściwość {_otherPropertyName} nie istnieje w klasie {validationContext.ObjectType.Name}.");
            }

            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance);

            if (!Equals(value, otherPropertyValue))
            {
                return new ValidationResult(ErrorMessage ?? $"Pola {validationContext.MemberName} i {_otherPropertyName} muszą być takie same.");
            }

            return ValidationResult.Success!;
        }
    }
}
