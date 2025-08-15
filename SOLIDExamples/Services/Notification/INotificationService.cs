using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Notification
{
    public interface IEmailNotification
    {
        Task<NotificationResult> SendEmailAsync(NotificationRequest request);
    }

    public interface ISmsNotification
    {
        Task<NotificationResult> SendSmsAsync(NotificationRequest request);
    }

    public interface IPushNotification
    {
        Task<NotificationResult> SendPushAsync(NotificationRequest request);
    }

    // ISP-compliant: Only implement what you need
    public class SmsNotificationService : ISmsNotification
    {
        public async Task<NotificationResult> SendSmsAsync(NotificationRequest request)
            => await Task.FromResult(new NotificationResult { Success = true, Message = "SMS sent" });
    }

    public class EmailNotificationService : IEmailNotification
    {
        public async Task<NotificationResult> SendEmailAsync(NotificationRequest request)
            => await Task.FromResult(new NotificationResult { Success = true, Message = "Email sent" });
    }
}
