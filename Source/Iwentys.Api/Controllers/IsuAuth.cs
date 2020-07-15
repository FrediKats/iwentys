using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuth : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int code)
        {
            return Ok();
        }
    }
}