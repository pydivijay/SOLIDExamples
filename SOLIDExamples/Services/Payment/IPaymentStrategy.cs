using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Payment
{
    public interface IPaymentStrategy
    {
        PaymentMethodType PaymentMethod { get; }
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<bool> ValidatePaymentAsync(PaymentRequest request);
        decimal CalculateProcessingFee(decimal amount);
    }
}