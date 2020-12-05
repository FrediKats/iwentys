using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.Achievements.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementController(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        [HttpGet("for-student")]
        public ActionResult<List<AchievementDto>> GetForStudent(int studentId)
        {
            List<AchievementDto> achievements = _achievementRepository
                .ReadStudentAchievements()
                .Where(a => a.StudentId == studentId)
                .AsEnumerable().Select(AchievementDto.Wrap)
                .ToList();

            return Ok(achievements);
        }
    }
}