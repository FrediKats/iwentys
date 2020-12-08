using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly GuildService _guildService;
        private readonly GuildMemberService _guildMemberService;

        public GuildController(GuildService guildService, GuildMemberService guildMemberService)
        {
            _guildService = guildService;
            _guildMemberService = guildMemberService;
        }

        [HttpPost]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Create([FromBody] GuildCreateRequestDto arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            return Ok(await _guildService.CreateAsync(creator, arguments));
        }

        [HttpPut]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Update([FromBody] GuildUpdateRequestDto arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.UpdateAsync(user, arguments));
        }

        [HttpGet]
        public ActionResult<List<GuildProfileDto>> GetOverview([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_guildService.GetOverview(skip, take));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExtendedGuildProfileWithMemberDataDto>> Get(int id)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.GetAsync(id, user.Id));
        }

        [HttpGet("for-member")]
        public ActionResult<GuildProfileDto> GetForMember(int memberId)
        {
            return Ok(_guildService.FindStudentGuild(memberId));
        }

        [HttpPut("{guildId}/enter")]
        public async Task<ActionResult<GuildProfileDto>> Enter(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.EnterGuildAsync(user, guildId));
        }

        [HttpPut("{guildId}/request")]
        public async Task<ActionResult<GuildProfileDto>> SendRequest(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.RequestGuildAsync(user, guildId));
        }

        [HttpPut("{guildId}/leave")]
        public async Task<ActionResult> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.LeaveGuildAsync(user, guildId);
            return Ok();
        }

        [HttpGet("{guildId}/request")]
        public async Task<ActionResult<GuildMemberEntity[]>> GetGuildRequests(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildRequests(user, guildId));
        }

        [HttpGet("{guildId}/blocked")]
        public async Task<ActionResult<GuildMemberEntity[]>> GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildBlocked(user, guildId));
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public async Task<IActionResult> BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.BlockGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public async Task<IActionResult> UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.UnblockStudent(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public async Task<IActionResult> KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.KickGuildMemberAsync(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public async Task<IActionResult> AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.AcceptRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public async Task<IActionResult> RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.RejectRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPost("{guildId}/pinned")]
        public async Task<ActionResult<GithubRepositoryInfoDto>> AddPinnedProject([FromRoute] int guildId, [FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.AddPinnedRepositoryAsync(user, guildId, createProject.Owner, createProject.RepositoryName));
        }

        [HttpDelete("{guildId}/pinned/{repositoryId}")]
        public async Task<ActionResult> DeletePinnedProject(int guildId, long repositoryId)
        {
            //TODO: Need to rework all links between GithubRepository, Student project and PinnedRepository
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