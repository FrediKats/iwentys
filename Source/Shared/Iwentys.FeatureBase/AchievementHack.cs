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
                achievementHack.AchieveForGuild(guildAchievement.Achievement, guildAchievement.GuildId);
            }

            foreach (StudentAchievement achievement in provider.StudentAchievement)
            {
                achievementHack.Achieve(achievement.Achievement, achievement.StudentId);
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

        public void Achieve(Achievement achievement, int studentId)
        {
            if (_studentAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;

            _studentAchievementRepository.Insert(StudentAchievement.Create(studentId, achievement.Id));
        }

        public void AchieveForGuild(Achievement achievement, int guildId)
        {
            if (_guildAchievementRepository.Get().Any(s => s.AchievementId == achievement.Id && s.GuildId == guildId))
                return;

            _guildAchievementRepository.Insert(GuildAchievement.Create(guildId, achievement.Id));
        }
    }
}