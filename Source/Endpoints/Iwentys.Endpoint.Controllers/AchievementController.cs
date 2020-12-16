using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Achievements.Models;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        //TODO: move to service
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentAchievementEntity> _studentAchievementRepository;

        public AchievementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievementEntity>();
        }

        [HttpGet("for-student")]
        public ActionResult<List<AchievementDto>> GetForStudent(int studentId)
        {
            List<AchievementDto> achievements = _studentAchievementRepository
                .GetAsync()
                .Where(a => a.StudentId == studentId)
                .AsEnumerable().Select(AchievementDto.Wrap)
                .ToList();

            return Ok(achievements);
        }
    }
}