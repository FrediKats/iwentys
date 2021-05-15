using System.Threading.Tasks;
using Iwentys.Domain.Guilds.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.GuildRecruitments
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
        public async Task<ActionResult<GuildRecruitmentInfoDto>> Create([FromRoute] int guildId, [FromBody] GuildRecruitmentCreateArguments createArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CreateGuildRecruitment.Response response = await _mediator.Send(new CreateGuildRecruitment.Query(user, guildId, createArguments));
            return Ok(response.GuildRecruitment);
        }
    }
}