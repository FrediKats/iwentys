using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Tools;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Controllers
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
        public async Task<ActionResult<GuildTestTaskInfoResponse>> Accept([FromQuery]int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Accept(user, guildId);
            return Ok(testTask);
        }

        [HttpPut("submit")]
        public async Task<ActionResult<GuildTestTaskInfoResponse>> AcceptAsync([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Submit(user, guildId, projectOwner, projectName);
            return Ok(testTask);
        }

        [HttpPut("complete")]
        public async Task<ActionResult<GuildTestTaskInfoResponse>> Submit([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Complete(user, guildId, taskSolveOwnerId);
            return Ok(testTask);
        }
    }
}
