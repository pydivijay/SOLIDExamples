using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.Logging;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]/bad")]
    public class LoggerBadController : ControllerBase
    {
        private readonly LoggerServiceBad _logger;
        public LoggerBadController(LoggerServiceBad logger) => _logger = logger;

        [HttpPost]
        public async Task<IActionResult> Log([FromBody] LogRequest req)
            => Ok(await _logger.LogAsync(req));
    }
}
