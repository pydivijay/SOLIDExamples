using Microsoft.Extensions.Logging;

namespace SOLIDExamples.Services
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogError(string message, Exception exception = null);
        void LogWarning(string message);
    }

    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message, Exception exception = null)
        {
            _logger.LogError(exception, message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
