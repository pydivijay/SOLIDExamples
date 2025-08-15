using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.Notification;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]/bad")]
    public class NotificationBadController : ControllerBase
    {
        private readonly INotificationServiceBad _service;
        public NotificationBadController(INotificationServiceBad service) => _service = service;

        [HttpPost("sms")]
        public async Task<IActionResult> SendSms([FromBody] NotificationRequest req)
            => Ok(await _service.SendSmsAsync(req));
    }
}
