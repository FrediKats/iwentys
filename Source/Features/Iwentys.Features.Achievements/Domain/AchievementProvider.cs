using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Achievements.Entities;

namespace Iwentys.Features.Achievements.Domain
{
    public class AchievementProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<StudentAchievementEntity> _studentAchievementRepository;

        public AchievementProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievementEntity>();
        }

        public async Task Achieve(AchievementEntity achievement, int studentId)
        {
            if (_studentAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;
            
            await _studentAchievementRepository.InsertAsync(StudentAchievementEntity.Create(studentId, achievement.Id));
            await _unitOfWork.CommitAsync();
        }
    }
}