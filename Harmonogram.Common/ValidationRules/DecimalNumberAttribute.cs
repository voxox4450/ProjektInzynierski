using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Harmonogram.Common.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DecimalNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            string stringValue = value.ToString()!;

            if (!decimal.TryParse(stringValue, NumberStyles.Number, new CultureInfo("pl-PL"), out _))
            {
                return new ValidationResult(ErrorMessage ?? "Pole tekstowe może zawierać tylko cyfry i przecinki w poprawnym formacie liczbowym.");
            }

            return ValidationResult.Success!;
        }
    }
}
