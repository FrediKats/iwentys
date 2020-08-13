using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Types.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/guild/tribute")]
    [ApiController]
    public class GuildTributeController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildTributeController(IGuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpGet]
        public IActionResult GetPendingTributes()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.GetPendingTributes(user);
            return Ok();
        }

        [HttpGet("GetFroStudent")]
        public IActionResult GetStudentTributeResult()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.GetStudentTributeResult(user);
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult SendTribute([FromBody] int projectId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CreateTribute(user, projectId);
            return Ok();
        }

        [HttpPost("cancel")]
        public IActionResult CancelTribute([FromBody] int tributeId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CancelTribute(user, tributeId);
            return Ok();
        }

        [HttpPost("complete")]
        public IActionResult CompleteTribute([FromBody] TributeCompleteDto tributeCompleteDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CompleteTribute(user, tributeCompleteDto);
            return Ok();
        }
    }
}
