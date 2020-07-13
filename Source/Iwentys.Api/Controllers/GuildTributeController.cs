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
        public void GetPendingTributes()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.GetPendingTributes(user);
        }

        [HttpGet("GetFroStudent")]
        public void GetStudentTributeResult()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.GetStudentTributeResult(user);
        }

        [HttpPost("create")]
        public void SendTribute([FromBody] int projectId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CreateTribute(user, projectId);
        }

        [HttpPost("cancel")]
        public void CancelTribute([FromBody] int tributeId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CancelTribute(user, tributeId);
        }

        [HttpPost("complete")]
        public void CompleteTribute([FromBody] TributeCompleteDto tributeCompleteDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CompleteTribute(user, tributeCompleteDto);
        }
    }
}
