using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Notification
{
    // ISP-Violating Fat Interface
    public interface INotificationServiceBad
    {
        Task<NotificationResult> SendEmailAsync(NotificationRequest request);
        Task<NotificationResult> SendSmsAsync(NotificationRequest request);
        Task<NotificationResult> SendPushAsync(NotificationRequest request);
        Task<NotificationResult> SendSlackAsync(NotificationRequest request);
    }

    // Violates ISP: SMS service must implement all methods, even if not supported
    public class SmsNotificationServiceBad : INotificationServiceBad
    {
        public Task<NotificationResult> SendEmailAsync(NotificationRequest request)
            => throw new System.NotImplementedException("SMS service does not support email.");

        public async Task<NotificationResult> SendSmsAsync(NotificationRequest request)
            => await Task.FromResult(new NotificationResult { Success = true, Message = "SMS sent" });

        public Task<NotificationResult> SendPushAsync(NotificationRequest request)
            => throw new System.NotImplementedException("SMS service does not support push.");

        public Task<NotificationResult> SendSlackAsync(NotificationRequest request)
            => throw new System.NotImplementedException("SMS service does not support Slack.");
    }
}
