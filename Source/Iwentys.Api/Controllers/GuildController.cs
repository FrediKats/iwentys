using System.Collections.Generic;
using Iwentys.Api.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IGuildService _guildService;
        private readonly IGuildMemberService _guildMemberService;

        public GuildController(IGuildService guildService, IGuildMemberService guildMemberService)
        {
            _guildService = guildService;
            _guildMemberService = guildMemberService;
        }

        [HttpPost]
        public ActionResult<GuildProfileShortInfoDto> Create([FromBody] GuildCreateArgumentDto arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            return Ok(_guildService.Create(creator, arguments));
        }

        [HttpPost("update")]
        public ActionResult<GuildProfileShortInfoDto> Update([FromBody] GuildUpdateArgumentDto arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.Update(user, arguments));
        }

        [HttpGet]
        public ActionResult<IEnumerable<GuildProfilePreviewDto>> GetOverview([FromQuery]int skip = 0, [FromQuery]int take = 20)
        {
            return Ok(_guildService.GetOverview(skip, take));
        }

        [HttpGet("{id}")]
        public ActionResult<GuildProfileDto> Get(int id)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.Get(id, user.Id));
        }

        [HttpPut("{guildId}/enter")]
        public ActionResult<GuildProfileDto> Enter(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildMemberService.EnterGuild(user, guildId));
        }

        [HttpPut("{guildId}/request")]
        public ActionResult<GuildProfileDto> SendRequest(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildMemberService.RequestGuild(user, guildId));
        }

        [HttpPut("{guildId}/leave")]
        public ActionResult<GuildProfileDto> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildMemberService.LeaveGuild(user, guildId));
        }

        [HttpGet("{guildId}/request")]
        public ActionResult<GuildMemberEntity[]> GetGuildRequests(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildMemberService.GetGuildRequests(user, guildId));
        }

        [HttpGet("{guildId}/blocked")]
        public ActionResult<GuildMemberEntity[]> GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildMemberService.GetGuildBlocked(user, guildId));
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public IActionResult BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            _guildMemberService.BlockGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public IActionResult UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            _guildMemberService.UnblockStudent(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public IActionResult KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            _guildMemberService.KickGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public IActionResult AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            _guildMemberService.AcceptRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public IActionResult RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            _guildMemberService.RejectRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPost("{guildId}/pinned")]
        public ActionResult<GithubRepository> AddPinnedProject([FromRoute]int guildId, [FromBody]CreateProjectDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.AddPinnedRepository(user, guildId, createProject.Owner, createProject.RepositoryName));
        }

        [HttpDelete("{guildId}/pinned")]
        public ActionResult DeletePinnedProject(int guildId, [FromBody] int repositoryId)
        {
            //TODO: Need to rework all links between GithubRepository, Student project and PinnedRepository
            AuthorizedUser user = this.TryAuthWithToken();
            _guildService.UnpinProject(user, repositoryId);
            return Ok();
        }
    }
}