using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/GuildTestTask")]
    [ApiController]
    public class GuildTestTaskServiceController : ControllerBase
    {
        private readonly GuildTestTaskService _guildTestTaskService;

        public GuildTestTaskServiceController(GuildTestTaskService guildTestTaskService)
        {
            _guildTestTaskService = guildTestTaskService;
        }

        [HttpGet(nameof(GetByGuildId))]
        public ActionResult<List<GuildTestTaskInfoResponse>> GetByGuildId([FromQuery] int guildId)
        {
            return Ok(_guildTestTaskService.GetResponses(guildId));
        }

        [HttpPut(nameof(Accept))]
        public async Task<ActionResult<GuildTestTaskInfoResponse>> Accept([FromQuery]int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Accept(user, guildId);
            return Ok(testTask);
        }

        [HttpPut(nameof(Submit))]
        public async Task<ActionResult<GuildTestTaskInfoResponse>> Submit([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Submit(user, guildId, projectOwner, projectName);
            return Ok(testTask);
        }

        [HttpPut(nameof(Complete))]
        public async Task<ActionResult<GuildTestTaskInfoResponse>> Complete([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildTestTaskInfoResponse testTask = await _guildTestTaskService.Complete(user, guildId, taskSolveOwnerId);
            return Ok(testTask);
        }
    }
}
