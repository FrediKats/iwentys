using System.Collections.Generic;

using Iwentys.Api.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;

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

        [HttpGet("pending")]
        public ActionResult<List<TributeInfoResponse>> GetPendingTributes()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.GetPendingTributes(user));
        }

        [HttpGet("get-for-student")]
        public ActionResult<List<TributeInfoResponse>> GetStudentTributeResult()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.GetStudentTributeResult(user));
        }

        [HttpPost("create")]
        public ActionResult<TributeInfoResponse> SendTribute([FromBody] CreateProjectRequest createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.CreateTribute(user, createProject));
        }

        [HttpPut("cancel")]
        public ActionResult<TributeInfoResponse> CancelTribute([FromBody] long tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.CancelTribute(user, tributeId));
        }

        [HttpPut("complete")]
        public ActionResult<TributeInfoResponse> CompleteTribute([FromBody] TributeCompleteRequest tributeCompleteRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.CompleteTribute(user, tributeCompleteRequest));
        }
    }
}
