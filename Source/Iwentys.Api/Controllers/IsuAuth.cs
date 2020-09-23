using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuthController : ControllerBase
    {
        private readonly ILogger<IsuAuthController> _logger;

        public IsuAuthController(ILogger<IsuAuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get(string code)
        {
            _logger.LogInformation($"Get code for isu auth: {code}");
            return Ok(code);
        }
    }
}