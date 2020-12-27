using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Guilds.Tributes.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/guild/tribute")]
    [ApiController]
    public class GuildTributeController : ControllerBase
    {
        private readonly GuildTributeService _guildService;

        public GuildTributeController(GuildTributeService guildService)
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

        [HttpGet("get-for-guild")]
        public ActionResult<List<TributeInfoResponse>> GetGuildTribute(int guildId)
        {
            return Ok(_guildService.GetGuildTributes(guildId));
        }

        [HttpPost("create")]
        public async Task<ActionResult<TributeInfoResponse>> SendTribute([FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CreateTribute(user, createProject);
            return Ok(tributes);
        }

        [HttpPut("cancel")]
        public async Task<ActionResult<TributeInfoResponse>> CancelTribute(long tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CancelTribute(user, tributeId);
            return Ok(tributes);
        }

        [HttpPut("complete")]
        public async Task<ActionResult<TributeInfoResponse>> CompleteTribute([FromBody] TributeCompleteRequest tributeCompleteRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CompleteTribute(user, tributeCompleteRequest);
            return Ok(tributes);
        }

        [HttpGet("get-for-student/active")]
        public async Task<ActionResult<TributeInfoResponse>> FindStudentActiveTribute()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.FindStudentActiveTribute(user);
            if (tributes is null)
                return NotFound();
            return Ok(tributes);
        }
    }
}
