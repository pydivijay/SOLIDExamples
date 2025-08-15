using System.Threading.Tasks;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.Logging
{
    // Abstraction
    public interface ILoggerService
    {
        Task<LogResult> LogAsync(LogRequest request);
    }

    // Low-level module implements abstraction
    public class ConsoleLoggerService : ILoggerService
    {
        public async Task<LogResult> LogAsync(LogRequest request)
        {
            System.Console.WriteLine($"[Console] {request.Message}");
            return await Task.FromResult(new LogResult { Success = true, Message = "Logged to console" });
        }
    }

    public class FileLoggerService : ILoggerService
    {
        public async Task<LogResult> LogAsync(LogRequest request)
        {
            // For demo, just simulate file logging
            await Task.Delay(10);
            return new LogResult { Success = true, Message = "Logged to file (simulated)" };
        }
    }
}
