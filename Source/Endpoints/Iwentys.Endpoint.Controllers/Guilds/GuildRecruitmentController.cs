using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Recruitment;
using Iwentys.Features.Guilds.Services;
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
        public async Task<ActionResult<GuildRecruitment>> Create([FromRoute] int guildId, [FromBody] GuildRecruitmentCreateArguments createArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitment recruitment = await _guildRecruitmentService.Create(guildId, user, createArguments);
            return Ok(recruitment);
        }
    }
}