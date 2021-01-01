using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/guild/recruitment")]
    [ApiController]
    public class GuildRecruitmentController : ControllerBase
    {
        private readonly GuildRecruitmentService _guildRecruitmentService;

        public GuildRecruitmentController(GuildRecruitmentService guildRecruitmentService)
        {
            _guildRecruitmentService = guildRecruitmentService;
        }

        [HttpPost("{guildId}/")]
        public async Task<ActionResult<GuildRecruitment>> Create([FromRoute] int guildId, [FromQuery] string description)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitment recruitment = await _guildRecruitmentService.Create(guildId, user.Id, description);
            return Ok(recruitment);
        }
    }
}