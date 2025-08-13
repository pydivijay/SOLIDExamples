using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    // Payment Processing Models
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Email { get; set; }
        public string CardToken { get; set; }
        public string AccountNumber { get; set; }
        public string WalletAddress { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public decimal ProcessingFee { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentMethodType
    {
        CreditCard,
        PayPal,
        Stripe,
        BankTransfer,
        Cryptocurrency,
        ApplePay,
        GooglePay
    }

    public class PaymentMethodInfo
    {
        public PaymentMethodType PaymentMethod { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProcessPaymentDto
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Email { get; set; }
        public string CardToken { get; set; }
        public string AccountNumber { get; set; }
        public string WalletAddress { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
