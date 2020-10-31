using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Models.Entities.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/guild/recruitment")]
    [ApiController]
    public class GuildRecruitmentController : ControllerBase
    {
        private GuildRecruitmentService _guildRecruitmentService;

        public GuildRecruitmentController(GuildRecruitmentService guildRecruitmentService)
        {
            _guildRecruitmentService = guildRecruitmentService;
        }

        [HttpPost("{guildId}/")]
        public async Task<ActionResult<GuildRecruitmentEntity>> Create([FromRoute] int guildId, [FromQuery] string description)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitmentEntity recruitmentEntity = await _guildRecruitmentService.Create(guildId, user.Id, description);
            return Ok(recruitmentEntity);
        }
    }
}