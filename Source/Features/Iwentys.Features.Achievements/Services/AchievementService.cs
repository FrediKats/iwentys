using System.Collections.Generic;
using System.Linq;
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

        public List<AchievementDto> GetForStudent(int studentId)
        {
            return _studentAchievementRepository
                .Get()
                .Where(a => a.StudentId == studentId)
                .Select(AchievementDto.FromEntity)
                .ToList();
        }
    }
}