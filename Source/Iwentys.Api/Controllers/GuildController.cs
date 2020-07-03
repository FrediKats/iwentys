using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
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

        [HttpPost("id/VotingForTotem}")]
        public void VotingForLeader(int guildId, GuildLeaderVotingCreateDto guildLeaderVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForLeader(user, guildId, guildLeaderVotingCreateDto);
        }

        [HttpPost("id/VotingForTotem}")]
        public void VotingForTotem(int guildId, GuildTotemVotingCreateDto guildTotemVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForTotem(user, guildId, guildTotemVotingCreateDto);
        }

        [HttpPost("id/VotingForTotem}")]
        public void SendTribute(int guildId, int repositoryId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.SendTribute(user, guildId, repositoryId);
        }
    }
}