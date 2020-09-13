using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Guilds;
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

        [HttpGet]
        public ActionResult<List<GuildTestTaskInfoDto>> Get([FromQuery] int guildId)
        {
            return Ok(_guildTestTaskService.Get(guildId));
        }

        [HttpGet("accept")]
        public ActionResult<GuildTestTaskInfoDto> Accept([FromQuery]int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildTestTaskService.Accept(user, guildId));
        }

        [HttpGet("accept")]
        public ActionResult<GuildTestTaskInfoDto> Accept([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildTestTaskService.Submit(user, guildId, projectOwner, projectName));
        }

        [HttpGet("complete")]
        public ActionResult<GuildTestTaskInfoDto> Submit([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildTestTaskService.Complete(user, guildId, taskSolveOwnerId));
        }
    }
}
