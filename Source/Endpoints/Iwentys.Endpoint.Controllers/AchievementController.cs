using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.Repositories;
using Iwentys.Features.Achievements.ViewModels;
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
        public async Task<ActionResult<List<AchievementViewModel>>> GetForStudent(int studentId)
        {
            List<AchievementViewModel> achievements = _achievementRepository
                .ReadStudentAchievements()
                .Where(a => a.StudentId == studentId)
                .AsEnumerable().Select(AchievementViewModel.Wrap)
                .ToList();

            return Ok(achievements);
        }
    }
}