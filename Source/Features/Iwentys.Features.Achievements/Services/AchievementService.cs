using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Achievements.Models;

namespace Iwentys.Features.Achievements.Services
{
    public class AchievementService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentAchievementEntity> _studentAchievementRepository;

        public AchievementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievementEntity>();
        }

        public async Task<List<AchievementDto>> GetForStudent(int studentId)
        {
            List<AchievementDto> achievements = _studentAchievementRepository
                .GetAsync()
                .Where(a => a.StudentId == studentId)
                .Select(AchievementDto.FromEntity)
                .ToList();

            return achievements;
        }
    }
}