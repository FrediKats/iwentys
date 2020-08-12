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
        public GuildProfileShortInfoDto Create([FromBody] GuildCreateArgumentDto arguments)
        {
            AuthorizedUser creator = AuthorizedUser.DebugAuth();
            return _guildService.Create(creator, arguments);
        }

        [HttpPost("update")]
        public GuildProfileShortInfoDto Update([FromBody] GuildUpdateArgumentDto arguments)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.Update(user, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfilePreviewDto> GetOverview([FromQuery]int skip = 0, [FromQuery]int take = 20)
        {
            return _guildService.GetOverview(skip, take);
        }

        [HttpGet("{id}")]
        public GuildProfileDto Get(int id)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.Get(id, user.Id);
        }

        [HttpPut("{guildId}/enter")]
        public GuildProfileDto Enter(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.EnterGuild(user, guildId);
        }

        [HttpPut("{guildId}/request")]
        public GuildProfileDto SendRequest(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.RequestGuild(user, guildId);
        }

        [HttpPut("{guildId}/leave")]
        public GuildProfileDto Leave(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.LeaveGuild(user, guildId);
        }

        [HttpGet("{guildId}/request")]
        public GuildMember[] GetGuildRequests(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.GetGuildRequests(user, guildId);
        }

        [HttpGet("{guildId}/blocked")]
        public GuildMember[] GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.GetGuildBlocked(user, guildId);
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public void BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.BlockGuildMember(user, guildId, memberId);
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public void UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.UnblockStudent(user, guildId, studentId);
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public void KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.KickGuildMember(user, guildId, memberId);
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public void AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.AcceptRequest(user, guildId, studentId);
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public void RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.RejectRequest(user, guildId, studentId);
        }

        [HttpPost("{guildId}/VotingForLeader")]
        public void VotingForLeader(int guildId, [FromBody] GuildLeaderVotingCreateDto guildLeaderVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForLeader(user, guildId, guildLeaderVotingCreateDto);
        }

        [HttpPost("{guildId}/VotingForTotem")]
        public void VotingForTotem(int guildId, [FromBody] GuildTotemVotingCreateDto guildTotemVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForTotem(user, guildId, guildTotemVotingCreateDto);
        }

        [HttpPost("{guildId}/SetTotem")]
        public void SetTotem(int guildId, [FromBody] int totemId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.SetTotem(user, guildId, totemId);
        }

        [HttpPost("{guildId}/pinned")]
        public GithubRepository AddPinnedProject(int guildId, [FromBody] string repositoryUrl)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.AddPinnedRepository(user, guildId, repositoryUrl);
        }

        [HttpDelete("{guildId}/pinned")]
        public GithubRepository DeletePinnedProject(int guildId, [FromBody] string repositoryUrl)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.DeletePinnedRepository(user, guildId, repositoryUrl);
        }
    }
}