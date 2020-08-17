using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Github;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpPost]
        public ActionResult<GuildProfileShortInfoDto> Create([FromBody] GuildCreateArgumentDto arguments)
        {
            AuthorizedUser creator = AuthorizedUser.DebugAuth();
            return Ok(_guildService.Create(creator, arguments));
        }

        [HttpPost("update")]
        public ActionResult<GuildProfileShortInfoDto> Update([FromBody] GuildUpdateArgumentDto arguments)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
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
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.Get(id, user.Id));
        }

        [HttpPut("{guildId}/enter")]
        public ActionResult<GuildProfileDto> Enter(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.EnterGuild(user, guildId));
        }

        [HttpPut("{guildId}/request")]
        public ActionResult<GuildProfileDto> SendRequest(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.RequestGuild(user, guildId));
        }

        [HttpPut("{guildId}/leave")]
        public ActionResult<GuildProfileDto> Leave(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.LeaveGuild(user, guildId));
        }

        [HttpGet("{guildId}/request")]
        public ActionResult<GuildMember[]> GetGuildRequests(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.GetGuildRequests(user, guildId));
        }

        [HttpGet("{guildId}/blocked")]
        public ActionResult<GuildMember[]> GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.GetGuildBlocked(user, guildId));
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public IActionResult BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.BlockGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public IActionResult UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.UnblockStudent(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public IActionResult KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.KickGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public IActionResult AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.AcceptRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public IActionResult RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.RejectRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPost("{guildId}/VotingForLeader")]
        public IActionResult VotingForLeader(int guildId, [FromBody] GuildLeaderVotingCreateDto guildLeaderVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForLeader(user, guildId, guildLeaderVotingCreateDto);
            return Ok();
        }

        [HttpPost("{guildId}/pinned")]
        public ActionResult<GithubRepository> AddPinnedProject([FromRoute]int guildId, [FromBody]CreatePinnedRepositoryDto createPinnedRepository)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_guildService.AddPinnedRepository(user, guildId, createPinnedRepository.Owner, createPinnedRepository.RepositoryName));
        }

        [HttpDelete("{guildId}/pinned")]
        public ActionResult DeletePinnedProject(int guildId, [FromBody] int repositoryId)
        {
            //TODO: check
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.UnpinProject(user, repositoryId);
            return Ok();
        }
    }
}