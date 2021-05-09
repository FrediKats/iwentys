using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application
{
    public class AchievementHack
    {
        public static async Task ProcessAchievement(AchievementProvider provider, IUnitOfWork unitOfWork)
        {
            var achievementHack = new AchievementHack(unitOfWork);
            foreach (GuildAchievement guildAchievement in provider.GuildAchievement)
            {
                achievementHack.AchieveForGuild(guildAchievement.AchievementId, guildAchievement.GuildId);
            }

            foreach (StudentAchievement achievement in provider.StudentAchievement)
            {
                achievementHack.Achieve(achievement.AchievementId, achievement.StudentId);
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

        public void Achieve(int achievementId, int studentId)
        {
            if (_studentAchievementRepository.Get().Any(s => s.AchievementId == achievementId && s.StudentId == studentId))
                return;

            _studentAchievementRepository.Insert(StudentAchievement.Create(studentId, achievementId));
        }

        public void AchieveForGuild(int achievementId, int guildId)
        {
            if (_guildAchievementRepository.Get().Any(s => s.AchievementId == achievementId && s.GuildId == guildId))
                return;

            _guildAchievementRepository.Insert(GuildAchievement.Create(guildId, achievementId));
        }
    }
}