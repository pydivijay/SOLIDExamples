using System;
using System.Collections.Generic;

namespace SOLIDExamples.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ApiResponse<T> CreateSuccess(T data) => new ApiResponse<T>
        {
            Success = true,
            Data = data
        };

        public static ApiResponse<T> CreateFailure(IEnumerable<string> errors) => new ApiResponse<T>
        {
            Success = false,
            Errors = new List<string>(errors)
        };
    }
}
