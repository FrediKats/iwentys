using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
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
        public GuildProfileDto Create([FromBody] GuildCreateArgumentDto arguments)
        {
            AuthorizedUser creator = AuthorizedUser.DebugAuth();
            return _guildService.Create(creator, arguments);
        }

        [HttpPost("update")]
        public GuildProfileDto Update([FromBody] GuildUpdateArgumentDto arguments)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.Update(user, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfileDto> Get()
        {
            return _guildService.Get();
        }

        [HttpGet("{id}")]
        public GuildProfileDto Get(int id, int? userId)
        {
            return _guildService.Get(id, userId);
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