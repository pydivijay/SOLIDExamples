using SOLIDExamples.Models;

namespace SOLIDExamples.Services
{
    public interface IUserValidator
    {
        ValidationResult ValidateRegistration(UserRegistrationDto userDto);
    }

    public class UserValidator : IUserValidator
    {
        public ValidationResult ValidateRegistration(UserRegistrationDto userDto)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(userDto.Email) || !userDto.Email.Contains("@"))
            {
                result.IsValid = false;
                result.Errors.Add("Invalid email format");
            }

            if (string.IsNullOrEmpty(userDto.Password) || userDto.Password.Length < 6)
            {
                result.IsValid = false;
                result.Errors.Add("Password must be at least 6 characters");
            }

            if (string.IsNullOrEmpty(userDto.Name))
            {
                result.IsValid = false;
                result.Errors.Add("Name is required");
            }

            return result;
        }
    }
}
