using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/guild/test-task")]
    [ApiController]
    public class GuildTestTaskServiceController : ControllerBase
    {
        private readonly GuildTestTaskService _guildTestTaskService;

        public GuildTestTaskServiceController(GuildTestTaskService guildTestTaskService)
        {
            _guildTestTaskService = guildTestTaskService;
        }

        [HttpGet]
        public ActionResult<List<GuildTestTaskInfoResponse>> Get([FromQuery] int guildId)
        {
            return Ok(_guildTestTaskService.Get(guildId));
        }

        [HttpPut("accept")]
        public ActionResult<GuildTestTaskInfoResponse> Accept([FromQuery]int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildTestTaskService.Accept(user, guildId));
        }

        [HttpPut("submit")]
        public ActionResult<GuildTestTaskInfoResponse> Accept([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildTestTaskService.Submit(user, guildId, projectOwner, projectName));
        }

        [HttpPut("complete")]
        public ActionResult<GuildTestTaskInfoResponse> Submit([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildTestTaskService.Complete(user, guildId, taskSolveOwnerId));
        }
    }
}
