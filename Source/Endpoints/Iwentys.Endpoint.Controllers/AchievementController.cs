using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.Achievements.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Achievements")]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementService _achievementService;

        public AchievementController(AchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet("students/{studentId}")]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetForStudent(int studentId)
        {
            List<AchievementInfoDto> achievementDtos = await _achievementService.GetForStudent(studentId);
            return Ok(achievementDtos);
        }

        [HttpGet("guilds/{guildId}")]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetForGuild(int guildId)
        {
            List<AchievementInfoDto> achievementDtos = await _achievementService.GetForGuild(guildId);
            return Ok(achievementDtos);
        }
    }
}