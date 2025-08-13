using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Payment
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(PaymentMethodType paymentMethod);
        IEnumerable<PaymentMethodType> GetSupportedPaymentMethods();
    }

    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<PaymentMethodType, Type> _strategies;

        public PaymentStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _strategies = new Dictionary<PaymentMethodType, Type>
            {
                { PaymentMethodType.CreditCard, typeof(CreditCardPaymentStrategy) },
                { PaymentMethodType.PayPal, typeof(PayPalPaymentStrategy) },
                { PaymentMethodType.Stripe, typeof(StripePaymentStrategy) },
                { PaymentMethodType.BankTransfer, typeof(BankTransferPaymentStrategy) },
                { PaymentMethodType.Cryptocurrency, typeof(CryptocurrencyPaymentStrategy) },
                { PaymentMethodType.ApplePay, typeof(ApplePayPaymentStrategy) }
            };
        }

        public IPaymentStrategy GetPaymentStrategy(PaymentMethodType paymentMethod)
        {
            if (!_strategies.TryGetValue(paymentMethod, out var strategyType))
            {
                throw new NotSupportedException($"Payment method {paymentMethod} is not supported");
            }
            return (IPaymentStrategy)_serviceProvider.GetService(strategyType)
                   ?? (IPaymentStrategy)Activator.CreateInstance(strategyType);
        }

        public IEnumerable<PaymentMethodType> GetSupportedPaymentMethods()
        {
            return _strategies.Keys;
        }
    }
}
