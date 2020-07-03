using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Guilds;
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

        [HttpPost]
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
        public GuildProfileDto Get(int id)
        {
            return _guildService.Get(id);
        }

        [HttpPost("{guildId}/VotingForTotem}")]
        public void VotingForLeader(int guildId, [FromBody] GuildLeaderVotingCreateDto guildLeaderVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForLeader(user, guildId, guildLeaderVotingCreateDto);
        }

        [HttpPost("{guildId}/VotingForTotem}")]
        public void VotingForTotem(int guildId, [FromBody] GuildTotemVotingCreateDto guildTotemVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForTotem(user, guildId, guildTotemVotingCreateDto);
        }

        [HttpPost("{guildId}/SendTribute}")]
        public void SendTribute(int guildId, [FromBody] int repositoryId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.SendTribute(user, guildId, repositoryId);
        }

        [HttpPost("{guildId}/CancelTribute}")]
        public void CancelTribute(int guildId, [FromBody] int tributeId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CancelTribute(user, tributeId);
        }

        [HttpPost("{guildId}/CompleteTribute}")]
        public void CompleteTribute(int guildId, [FromBody] TributeCompleteDto tributeCompleteDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.CompleteTribute(user, tributeCompleteDto);
        }
    }
}