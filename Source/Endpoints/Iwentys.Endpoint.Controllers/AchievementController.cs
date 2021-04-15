using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.Achievements.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementService _achievementService;

        public AchievementController(AchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByStudentId(int studentId)
        {
            List<AchievementInfoDto> achievementDtos = await _achievementService.GetForStudent(studentId);
            return Ok(achievementDtos);
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByGuildId(int guildId)
        {
            List<AchievementInfoDto> achievementDtos = await _achievementService.GetForGuild(guildId);
            return Ok(achievementDtos);
        }
    }
}