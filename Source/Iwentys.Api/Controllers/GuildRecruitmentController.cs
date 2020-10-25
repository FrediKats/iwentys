using Iwentys.Api.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services;
using Iwentys.Models.Entities.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
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
        public ActionResult<GuildRecruitmentEntity> Create([FromRoute] int guildId, [FromQuery] string description)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitmentEntity recruitmentEntity = _guildRecruitmentService.Create(guildId, user.Id, description);
            return Ok(recruitmentEntity);
        }
    }
}