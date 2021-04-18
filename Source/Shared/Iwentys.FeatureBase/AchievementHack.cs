using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Gamification;

namespace Iwentys.FeatureBase
{
    public class AchievementHack
    {
        public static async Task ProcessAchievement(AchievementProvider provider, IUnitOfWork unitOfWork)
        {
            var achievementHack = new AchievementHack(unitOfWork);
            foreach (GuildAchievement guildAchievement in provider.GuildAchievement)
            {
                await achievementHack.AchieveForGuild(guildAchievement.Achievement, guildAchievement.GuildId);
            }

            foreach (StudentAchievement achievement in provider.StudentAchievement)
            {
                await achievementHack.Achieve(achievement.Achievement, achievement.StudentId);
            }

            await unitOfWork.CommitAsync();
        }

        private readonly IGenericRepository<GuildAchievement> _guildAchievementRepository;
        private readonly IGenericRepository<StudentAchievement> _studentAchievementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementHack(IUnitOfWork unitOfWork)
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