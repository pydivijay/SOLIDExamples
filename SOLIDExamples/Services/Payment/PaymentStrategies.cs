using System;
using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Payment
{
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.CreditCard;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(1000);
            if (request.Amount > 5000)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = "Amount exceeds credit card limit",
                    PaymentMethod = PaymentMethod.ToString()
                };
            }
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"CC_{Guid.NewGuid().ToString()[..8]}",
                Message = "Credit card payment processed successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(200);
            return !string.IsNullOrEmpty(request.CardToken) && request.Amount > 0;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return amount * 0.029m + 0.30m;
        }
    }

    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.PayPal;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(1500);
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"PP_{Guid.NewGuid().ToString()[..8]}",
                Message = "PayPal payment processed successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(200);
            return !string.IsNullOrEmpty(request.Email) && request.Amount > 0;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return amount * 0.034m + 0.49m;
        }
    }

    public class StripePaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.Stripe;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(800);
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"pi_{Guid.NewGuid().ToString()[..10]}",
                Message = "Stripe payment processed successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(200);
            return !string.IsNullOrEmpty(request.CardToken) && request.Amount > 0;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return amount * 0.029m + 0.30m;
        }
    }

    public class BankTransferPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.BankTransfer;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(2000);
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"BT_{DateTime.Now:yyyyMMddHHmmss}",
                Message = "Bank transfer initiated successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(300);
            return !string.IsNullOrEmpty(request.AccountNumber) && request.Amount >= 10;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return Math.Max(1.00m, amount * 0.005m);
        }
    }

    public class CryptocurrencyPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.Cryptocurrency;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(3000);
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"0x{Guid.NewGuid().ToString().Replace("-", "")}",
                Message = "Cryptocurrency payment processed successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(500);
            return !string.IsNullOrEmpty(request.WalletAddress) && request.Amount > 0;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return amount * 0.015m;
        }
    }

    public class ApplePayPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethodType PaymentMethod => PaymentMethodType.ApplePay;

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(600);
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"AP_{Guid.NewGuid().ToString()[..8]}",
                Message = "Apple Pay payment processed successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount),
                PaymentMethod = PaymentMethod.ToString()
            };
        }

        public async Task<bool> ValidatePaymentAsync(PaymentRequest request)
        {
            await Task.Delay(100);
            return request.Amount > 0 && request.Amount <= 10000;
        }

        public decimal CalculateProcessingFee(decimal amount)
        {
            return amount * 0.025m;
        }
    }
}
