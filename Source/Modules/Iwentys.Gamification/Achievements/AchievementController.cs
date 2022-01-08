using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Achievements;
using Iwentys.Modules.Gamification.Achievements.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Gamification.Achievements
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AchievementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByStudentId(int studentId)
        {
            GetByStudentId.Response response = await _mediator.Send(new GetByStudentId.Query(studentId));
            return Ok(response.Achievements);
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByGuildId(int guildId)
        {
            GetByGuildId.Response response = await _mediator.Send(new GetByGuildId.Query(guildId));
            return Ok(response.Achievements);
        }
    }
}