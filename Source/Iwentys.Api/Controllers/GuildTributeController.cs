using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/guild/tribute")]
    [ApiController]
    public class GuildTributeController : ControllerBase
    {
        private readonly IGuildTributeService _guildService;

        public GuildTributeController(IGuildTributeService guildService)
        {
            _guildService = guildService;
        }

        [HttpGet]
        public ActionResult<TributeInfoDto[]> GetPendingTributes()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.GetPendingTributes(user));
        }

        [HttpGet("GetFroStudent")]
        public ActionResult<TributeInfoDto[]> GetStudentTributeResult()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.GetStudentTributeResult(user));
        }

        [HttpPost("create")]
        public ActionResult<TributeInfoDto> SendTribute([FromBody] int projectId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.CreateTribute(user, projectId));
        }

        [HttpPost("cancel")]
        public ActionResult<TributeInfoDto> CancelTribute([FromBody] int tributeId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.CancelTribute(user, tributeId));
        }

        [HttpPost("complete")]
        public ActionResult<TributeInfoDto> CompleteTribute([FromBody] TributeCompleteDto tributeCompleteDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.CompleteTribute(user, tributeCompleteDto));
        }
    }
}
