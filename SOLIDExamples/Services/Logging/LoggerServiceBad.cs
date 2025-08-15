using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Logging
{
    // High-level module depends on concrete ConsoleLogger directly (violates DIP)
    public class LoggerServiceBad
    {
        private readonly ConsoleLogger _logger = new ConsoleLogger();

        public async Task<LogResult> LogAsync(LogRequest request)
        {
            _logger.Log(request.Message);
            return await Task.FromResult(new LogResult { Success = true, Message = "Logged to console" });
        }
    }

    public class ConsoleLogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine($"[Console] {message}");
        }
    }
}
