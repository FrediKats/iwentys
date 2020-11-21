using System;
using System.Linq;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Features.Achievements
{
    public class AchievementProvider
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementProvider(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        public void Achieve(AchievementEntity achievement, int studentId)
        {
            if (_achievementRepository.ReadStudentAchievements().Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;

            _achievementRepository.CreateStudentAchievement(new StudentAchievementEntity { StudentId = studentId, AchievementId = achievement.Id, GettingTime = DateTime.UtcNow });
        }
    }
}