using System.ComponentModel.DataAnnotations;
namespace Harmonogram.Common.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            string password = value.ToString()!;

            if (password.Length < 8)
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsLower))
                return false;

            if (!password.Any(char.IsPunctuation))
                return false;

            if (!password.Any(char.IsNumber))
                return false;

            return true;
        }
    }
}
