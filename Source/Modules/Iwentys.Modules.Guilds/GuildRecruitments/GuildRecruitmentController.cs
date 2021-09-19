using System.Threading.Tasks;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Modules.Guilds.Dtos;
using Iwentys.Modules.Guilds.GuildRecruitments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Guilds.GuildRecruitments
{
    [Route("api/GuildRecruitment")]
    [ApiController]
    public class GuildRecruitmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildRecruitmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<GuildRecruitmentInfoDto>> Create(int guildId, [FromBody] GuildRecruitmentCreateArguments createArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CreateGuildRecruitment.Response response = await _mediator.Send(new CreateGuildRecruitment.Query(user, guildId, createArguments));
            return Ok(response.GuildRecruitment);
        }
    }
}