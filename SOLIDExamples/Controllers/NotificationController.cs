using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.Notification;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ISmsNotification _sms;
        private readonly IEmailNotification _email;
        public NotificationController(ISmsNotification sms, IEmailNotification email)
        {
            _sms = sms;
            _email = email;
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSms([FromBody] NotificationRequest req)
            => Ok(await _sms.SendSmsAsync(req));

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] NotificationRequest req)
            => Ok(await _email.SendEmailAsync(req));
    }
}
