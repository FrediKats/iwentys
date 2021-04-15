using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/guild")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly GuildService _guildService;

        public GuildController(GuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Create([FromBody] GuildCreateRequestDto arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            return Ok(await _guildService.Create(creator, arguments));
        }

        [HttpPut(nameof(Update))]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Update([FromBody] GuildUpdateRequestDto arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.Update(user, arguments));
        }

        [HttpGet(nameof(GetRanked))]
        public ActionResult<List<GuildProfileDto>> GetRanked([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_guildService.GetOverview(skip, take));
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<GuildProfileDto>> Get(int id)
        {
            return Ok(await _guildService.Get(id));
        }

        [HttpGet(nameof(GetByMemberId))]
        public ActionResult<GuildProfileDto> GetByMemberId(int memberId)
        {
            GuildProfileDto result = _guildService.FindStudentGuild(memberId);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPost(nameof(AddPinnedProject))]
        public async Task<ActionResult<GithubRepositoryInfoDto>> AddPinnedProject(int guildId, [FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.AddPinnedRepository(user, guildId, createProject.Owner, createProject.RepositoryName));
        }

        [HttpDelete(nameof(DeletePinnedProject))]
        public async Task<ActionResult> DeletePinnedProject(int guildId, long repositoryId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildService.UnpinProject(user, guildId, repositoryId);
            return Ok();
        }

        [HttpGet(nameof(GetGuildMemberLeaderBoard))]
        public async Task<ActionResult<GuildMemberLeaderBoardDto>> GetGuildMemberLeaderBoard(int guildId)
        {
            return Ok(await _guildService.GetGuildMemberLeaderBoard(guildId));
        }
    }
}