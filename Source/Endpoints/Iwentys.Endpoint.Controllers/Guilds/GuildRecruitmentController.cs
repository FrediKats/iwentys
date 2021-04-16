using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/GuildRecruitment")]
    [ApiController]
    public class GuildRecruitmentController : ControllerBase
    {
        private readonly GuildRecruitmentService _guildRecruitmentService;

        public GuildRecruitmentController(GuildRecruitmentService guildRecruitmentService)
        {
            _guildRecruitmentService = guildRecruitmentService;
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<GuildRecruitmentInfoDto>> Create([FromRoute] int guildId, [FromBody] GuildRecruitmentCreateArguments createArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitmentInfoDto recruitment = await _guildRecruitmentService.Create(guildId, user, createArguments);
            return Ok(recruitment);
        }
    }
}