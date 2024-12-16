using System.ComponentModel.DataAnnotations;

namespace ReportsSender4.Common.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class OnlyLettersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Wartość jest pusta");
            }

            string stringValue = value.ToString()!;
            if (!stringValue.All(char.IsLetter))
            {
                return new ValidationResult(ErrorMessage ?? "Pole tekstowe może zawierać tylko litery.");
            }

            return ValidationResult.Success!;
        }
    }
}