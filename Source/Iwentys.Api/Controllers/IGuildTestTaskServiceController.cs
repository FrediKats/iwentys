using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/guild/test-task")]
    [ApiController]
    public class GuildTestTaskServiceController : ControllerBase
    {
        private readonly IGuildTestTaskService _guildTestTaskService;

        public GuildTestTaskServiceController(IGuildTestTaskService guildTestTaskService)
        {
            _guildTestTaskService = guildTestTaskService;
        }

        [HttpGet("accept")]
        public ActionResult Accept([FromQuery]int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildTestTaskService.Accept(user, guildId);
            return Ok();
        }

        [HttpGet("accept")]
        public ActionResult Accept([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildTestTaskService.Submit(user, guildId, projectOwner, projectName);
            return Ok();
        }

        [HttpGet("complete")]
        public ActionResult Submit([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildTestTaskService.Complete(user, guildId, taskSolveOwnerId);
            return Ok();
        }
    }
}
