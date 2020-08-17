using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuth : ControllerBase
    {
        private readonly ILogger<IsuAuth> _logger;

        public IsuAuth(ILogger<IsuAuth> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(int code)
        {
            _logger.LogInformation($"Get code for isu auth: {code}");
            return Ok();
        }
    }
}