using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.Logging;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoggerController : ControllerBase
    {
        private readonly ILoggerService _logger;
        public LoggerController(ILoggerService logger) => _logger = logger;

        [HttpPost]
        public async Task<IActionResult> Log([FromBody] LogRequest req)
            => Ok(await _logger.LogAsync(req));
    }
}
