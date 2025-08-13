using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Payment
{
    public interface IPaymentProcessor
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentMethodType paymentMethod, PaymentRequest request);
        Task<IEnumerable<PaymentMethodInfo>> GetSupportedPaymentMethodsAsync();
    }

    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentStrategyFactory _strategyFactory;
        private readonly ILogger<PaymentProcessor> _logger;

        public PaymentProcessor(IPaymentStrategyFactory strategyFactory, ILogger<PaymentProcessor> logger)
        {
            _strategyFactory = strategyFactory;
            _logger = logger;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentMethodType paymentMethod, PaymentRequest request)
        {
            try
            {
                _logger.LogInformation($"Processing {paymentMethod} payment for amount {request.Amount}");
                var strategy = _strategyFactory.GetPaymentStrategy(paymentMethod);
                var isValid = await strategy.ValidatePaymentAsync(request);
                if (!isValid)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Payment validation failed",
                        PaymentMethod = paymentMethod.ToString()
                    };
                }
                var result = await strategy.ProcessPaymentAsync(request);
                _logger.LogInformation($"Payment processed: {result.Success}, Transaction: {result.TransactionId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing {paymentMethod} payment");
                return new PaymentResult
                {
                    Success = false,
                    Message = "An error occurred while processing payment",
                    PaymentMethod = paymentMethod.ToString()
                };
            }
        }

        public async Task<IEnumerable<PaymentMethodInfo>> GetSupportedPaymentMethodsAsync()
        {
            await Task.Delay(100);
            var supportedMethods = _strategyFactory.GetSupportedPaymentMethods();
            return supportedMethods.Select(method => new PaymentMethodInfo
            {
                PaymentMethod = method,
                Name = method.ToString(),
                Description = GetPaymentMethodDescription(method)
            });
        }

        private static string GetPaymentMethodDescription(PaymentMethodType method)
        {
            return method switch
            {
                PaymentMethodType.CreditCard => "Pay with Credit or Debit Card",
                PaymentMethodType.PayPal => "Pay with your PayPal account",
                PaymentMethodType.Stripe => "Secure payment processing via Stripe",
                PaymentMethodType.BankTransfer => "Direct bank account transfer",
                PaymentMethodType.Cryptocurrency => "Pay with cryptocurrency",
                PaymentMethodType.ApplePay => "Pay with Apple Pay",
                PaymentMethodType.GooglePay => "Pay with Google Pay",
                _ => method.ToString()
            };
        }
    }
}
