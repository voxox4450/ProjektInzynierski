using System.ComponentModel.DataAnnotations;

namespace Harmonogram.Common.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class OnlyNumbersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            string stringValue = value.ToString()!;

            if (!stringValue.All(char.IsDigit))
            {
                return new ValidationResult(ErrorMessage ?? "Pole tekstowe może zawierać tylko cyfry.");
            }

            return ValidationResult.Success!;
        }
    }
}
