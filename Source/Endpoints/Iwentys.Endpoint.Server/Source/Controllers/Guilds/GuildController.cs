using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Source.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.StudentFeature;
using Iwentys.Models;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Guilds
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
        public async Task<ActionResult<GuildProfileShortInfoDto>> Create([FromBody] GuildCreateRequest arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            return Ok(await _guildService.CreateAsync(creator, arguments));
        }

        [HttpPut]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Update([FromBody] GuildUpdateRequest arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.UpdateAsync(user, arguments));
        }

        [HttpGet]
        public ActionResult<List<GuildProfilePreviewDto>> GetOverview([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_guildService.GetOverview(skip, take));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuildProfileDto>> Get(int id)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.GetAsync(id, user.Id));
        }

        [HttpGet("for-member")]
        public async Task<ActionResult<GuildProfileDto>> GetForMember(int memberId)
        {
            return Ok(await _guildService.FindStudentGuild(memberId));
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
        public async Task<ActionResult<GuildProfileDto>> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.LeaveGuildAsync(user, guildId));
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
        public async Task<ActionResult<GithubRepository>> AddPinnedProject([FromRoute] int guildId, [FromBody] CreateProjectRequest createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildService.AddPinnedRepositoryAsync(user, guildId, createProject.Owner, createProject.RepositoryName));
        }

        [HttpDelete("{guildId}/pinned/{repositoryId}")]
        public async Task<ActionResult> DeletePinnedProject(int guildId, int repositoryId)
        {
            //TODO: Need to rework all links between GithubRepository, Student project and PinnedRepository
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildService.UnpinProject(user, repositoryId);
            return Ok();
        }
    }
}