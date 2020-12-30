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
        private readonly IGenericRepository<GuildAchievementEntity> _guildAchievementRepository;

        public AchievementProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievementEntity>();
            _guildAchievementRepository = _unitOfWork.GetRepository<GuildAchievementEntity>();
        }

        public async Task Achieve(AchievementEntity achievement, int studentId)
        {
            if (_studentAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;
            
            await _studentAchievementRepository.InsertAsync(StudentAchievementEntity.Create(studentId, achievement.Id));
            await _unitOfWork.CommitAsync();
        }

        public async Task AchieveForGuild(AchievementEntity achievement, int guildId)
        {
            if (_guildAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.GuildId == guildId))
                return;

            await _guildAchievementRepository.InsertAsync(GuildAchievementEntity.Create(guildId, achievement.Id));
            await _unitOfWork.CommitAsync();
        }
    }
}