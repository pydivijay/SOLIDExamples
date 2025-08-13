using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.Payment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentProcessor _paymentProcessor;

        public PaymentsController(IPaymentProcessor paymentProcessor)
        {
            _paymentProcessor = paymentProcessor;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDto dto)
        {
            if (!Enum.TryParse<PaymentMethodType>(dto.PaymentMethod, true, out var paymentMethod))
            {
                return BadRequest(new { Message = "Invalid payment method" });
            }

            var request = new PaymentRequest
            {
                Amount = dto.Amount,
                Currency = dto.Currency,
                Email = dto.Email,
                CardToken = dto.CardToken,
                AccountNumber = dto.AccountNumber,
                WalletAddress = dto.WalletAddress,
                Metadata = dto.Metadata ?? new Dictionary<string, object>()
            };

            var result = await _paymentProcessor.ProcessPaymentAsync(paymentMethod, request);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("methods")]
        public async Task<IActionResult> GetSupportedPaymentMethods()
        {
            var methods = await _paymentProcessor.GetSupportedPaymentMethodsAsync();
            return Ok(methods);
        }
    }
}
