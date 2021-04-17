using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Guilds.Guilds
{
    [Route("api/guild")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Create([FromBody] GuildCreateRequestDto arguments)
        {
            AuthorizedUser creator = this.TryAuthWithToken();
            CreateGuild.Response response = await _mediator.Send(new CreateGuild.Query(arguments, creator));
            return Ok(response.Guild);
        }

        [HttpPut(nameof(Update))]
        public async Task<ActionResult<GuildProfileShortInfoDto>> Update([FromBody] GuildUpdateRequestDto arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            UpdateGuild.Response response = await _mediator.Send(new UpdateGuild.Query(user, arguments));
            return Ok(response.Guild);
        }

        [HttpGet(nameof(GetRanked))]
        public async Task<ActionResult<List<GuildProfileDto>>> GetRanked([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            GetGuildRating.Response response = await _mediator.Send(new GetGuildRating.Query(skip, take));
            return Ok(response.Guilds);
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<GuildProfileDto>> Get(int id)
        {
            GetGuildById.Response response = await _mediator.Send(new GetGuildById.Query(id));
            return Ok(response.Guild);
        }

        [HttpGet(nameof(GetByMemberId))]
        public async Task<ActionResult<GuildProfileDto>> GetByMemberId(int memberId)
        {
            GetByMemberId.Response response = await _mediator.Send(new GetByMemberId.Query(memberId));
            GuildProfileDto result = response.Guild;
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpGet(nameof(GetGuildMemberLeaderBoard))]
        public async Task<ActionResult<GuildMemberLeaderBoardDto>> GetGuildMemberLeaderBoard(int guildId)
        {
            GetGuildMemberLeaderBoard.Response response = await _mediator.Send(new GetGuildMemberLeaderBoard.Query(guildId));
            return Ok(response.GuildMemberLeaderBoard);
        }
    }
}