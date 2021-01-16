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

        [HttpPost]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Create([FromBody] GuildCreateRequestDto arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            return Ok(await _guildService.Create(creator, arguments));
        }

        [HttpPut]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Update([FromBody] GuildUpdateRequestDto arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.Update(user, arguments));
        }

        [HttpGet]
        public ActionResult<List<GuildProfileDto>> GetOverview([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_guildService.GetOverview(skip, take));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuildProfileDto>> Get(int id)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.Get(id));
        }

        [HttpGet("for-member")]
        public ActionResult<GuildProfileDto> GetForMember(int memberId)
        {
            GuildProfileDto result = _guildService.FindStudentGuild(memberId);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPost("{guildId}/pinned")]
        public async Task<ActionResult<GithubRepositoryInfoDto>> AddPinnedProject([FromRoute] int guildId, [FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.AddPinnedRepository(user, guildId, createProject.Owner, createProject.RepositoryName));
        }

        [HttpDelete("{guildId}/pinned/{repositoryId}")]
        public async Task<ActionResult> DeletePinnedProject(int guildId, long repositoryId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildService.UnpinProject(user, guildId, repositoryId);
            return Ok();
        }

        [HttpGet("{guildId}/member-leaderboard")]
        public async Task<ActionResult<GuildMemberLeaderBoardDto>> GetGuildMemberLeaderBoard(int guildId)
        {
            return Ok(await _guildService.GetGuildMemberLeaderBoard(guildId));
        }
    }
}