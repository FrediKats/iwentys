using System;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Core.Gamification
{
    public class AchievementProvider
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public AchievementProvider(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public void Achieve(AchievementModel achievement, int studentId)
        {
            if (_databaseAccessor.Context.StudentAchievements.Any(s => s.AchievementId == achievement.Id && s.StudentId == studentId))
                return;

            _databaseAccessor.Context.StudentAchievements.Add(new StudentAchievementEntity {StudentId = studentId, AchievementId = achievement.Id, GettingTime = DateTime.UtcNow});
            _databaseAccessor.Context.SaveChanges();
        }
    }
}