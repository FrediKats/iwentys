using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;

namespace Iwentys.Domain.Gamification
{
    public class AchievementProvider
    {
        private readonly IGenericRepository<GuildAchievement> _guildAchievementRepository;
        private readonly IGenericRepository<StudentAchievement> _studentAchievementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _guildAchievementRepository = _unitOfWork.GetRepository<GuildAchievement>();
            _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievement>();
        }

        public async Task Achieve(Achievement achievement, int studentId)
        {
            if (_studentAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;

            await _studentAchievementRepository.InsertAsync(StudentAchievement.Create(studentId, achievement.Id));
        }

        public async Task AchieveForGuild(Achievement achievement, int guildId)
        {
            if (_guildAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.GuildId == guildId))
                return;

            await _guildAchievementRepository.InsertAsync(GuildAchievement.Create(guildId, achievement.Id));
        }
    }
}